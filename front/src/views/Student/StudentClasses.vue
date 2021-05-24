<template>
  <div style="width: 100%">

    <v-card class="pl-2 pr-2"   style="position: fixed; width:100%;margin:-16px;z-index: 100; border-radius: 0px" :elevation="elevation">

      <v-card-actions style="text-align: right;width: 100%">

        <div>
          <span style="font-size: 25px">Классы</span>
        </div>
        <div class="search">
          <v-autocomplete
              color="primary"
              class="mr-1 ml-5 searchInput"
              v-model="model"
              :items="$store.state.classes.map((c) =>{return {text:c.title,value:c.id}})"
              @change="$router.push('class/'+model)"
              label="Поиск по названию"
              persistent-hint
          ></v-autocomplete>
        </div>



      </v-card-actions>
    </v-card >

    <div style="padding-top: 70px;">
      <v-row  v-if="this.$store.state.classes.length>0" dense>
        <v-col
            v-for="card in this.$store.state.classes"
            :key="card.title"
            :cols="getNCols()"
        >
          <v-card class="m-2" elevation="4">
            <v-img
                style="cursor: pointer"
                @click="$router.push('/student/class/'+card.id)"
                :src="require('@/assets/imgs/class'+ Math.round((card.id % 5))+'.jpg')"
                class="white--text align-end"
                gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
                height="150px"
            >
              <v-card-title v-text="card.title"></v-card-title>
            </v-img>

            <v-card-actions>
              <v-list-item two-line>

                <v-list-item-avatar large color="orange">{{card.teacherName.toUpperCase()[0]}}</v-list-item-avatar>
                <v-list-item-content>
                  <v-list-item-title>{{ card.teacherName }}</v-list-item-title>
                  <v-list-item-subtitle>
                    {{card.teacherEmail}}
                  </v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
              <v-spacer></v-spacer>
<!--              <v-btn icon-->
<!--                     color="primary">-->
<!--                <v-icon>mdi-delete </v-icon>-->
<!--              </v-btn>-->
              <v-btn
                  icon
                  color="primary"
                  @click="showCode(card)">
                <v-icon>mdi-share-variant</v-icon>
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </div>


    <v-dialog
        v-model="dialog"
        persistent
        max-width="500px"
    >
      <template v-slot:activator="{ on, attrs }">
        <v-btn
            x-large
            fab
            style="position:fixed;bottom: 30px;right: 30px"
            color="primary"
            elevation="4"
            v-bind="attrs"
            v-on="on"
        >
          <v-icon x-large>mdi-plus</v-icon>
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          <span class="headline">Присоединиться к классу</span>
        </v-card-title>
        <v-card-text>
          <v-form ref="form"
                  v-model="valid"
                  lazy-validation>
            <v-text-field
                label="Код класса"
                v-model="classCode"
                :rules="titleRules"
                required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
              color="blue darken-1"
              text
              @click="dialog = false"
          >
            Закрыть
          </v-btn>
          <v-btn
              color="blue darken-1"
              text
              @click="addClass()"
          >
            Присоединиться
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>


  </div>
</template>

<script>
import Swal from "sweetalert2";
export default {
  name: "StudentClasses",
  data(){
    return{
      elevation:0,
      valid: true,
      titleRules: [
        v => !!v || 'Код класса не указан'
      ],
      isEditing: false,
      model: null,
      dialog: false,

      classCode:null
    }
  },

  destroyed () {
    window.removeEventListener('scroll', this.handleScroll);
  },

  async created() {
    window.addEventListener('scroll', this.handleScroll);
    await this.$store.dispatch("GetClassesStudent")
    console.log(this.$store.state.classes)
  },

  methods:{

    handleScroll (event) {
      if(window.scrollY > 0){
        this.elevation = 5
      }else {
        this.elevation = 0
      }
    },

    async addClass(){
      if(this.$refs.form.validate()){
        await this.$store.dispatch("JoinToClassStudent",this.classCode)
        this.dialog = false;
      }
    },

    showCode(card){
      Swal.fire(
          'Поделитесь кодом класса',
          card.code,
      )
    },

    getNCols(){
      if(this.$store.state.windowWidth<600){
        return 12
      }
      if(this.$store.state.windowWidth<1264){
        return 6
      }
      if(this.$store.state.windowWidth>1264){
        return 4
      }
    }
  }
}
</script>

<style scoped>

</style>