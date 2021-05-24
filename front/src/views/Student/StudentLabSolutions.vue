<template>
  <div style="width: 100%">

    <v-card class="pl-2 pr-2"   style="position: fixed; width:100%;margin:-16px;z-index: 100; border-radius: 0px" :elevation="elevation">

      <v-card-actions style="text-align: right;width: 100%">

        <div>
          <span style="font-size: 25px">Задания</span>
        </div>
        <div class="search">
          <v-autocomplete
              color="primary"
              class="mr-1 ml-5 searchInput"
              v-model="model"
              :items="items"
              @change="goToLab(model)"
              label="Поиск по названию"
              persistent-hint
          ></v-autocomplete>
        </div>



      </v-card-actions>
    </v-card >

    <div style="padding-top: 70px;">
      <v-card color="basil" flat>
        <v-list
            subheader
            two-line
        >
          <v-list-item-group
              v-model="labs"
              color="primary"
          >

            <v-subheader inset>Текущие лабораторные</v-subheader>

            <v-list-item
                :value="sol.lab.id"
                v-for="sol in $store.state.solutions.activeSolutions"
                :key="sol.lab.id"
                @click="goToLab(sol)">
              <template v-slot:default="{ active }">
                <v-list-item-avatar color="primary" >
                  <v-icon  dark >mdi-clipboard-text</v-icon>
                </v-list-item-avatar>
                <v-list-item-content>
                  <v-list-item-title>{{sol.lab.title}}</v-list-item-title>
                  <v-list-item-subtitle>{{getDate(sol.lab.deadline)}}</v-list-item-subtitle>
                </v-list-item-content>

                <v-list-item-icon v-if="sol.grade != null">
                  {{sol.grade}} / {{sol.lab.maxGrade}}
                  <v-icon color="primary">
                    mdi-star
                  </v-icon>
                </v-list-item-icon>

                <v-list-item-icon>
                  <div :class="''+getStatusStyle(sol)">{{getStatus(sol)}}</div>
                </v-list-item-icon>

              </template>

            </v-list-item>

            <v-divider></v-divider>
            <v-subheader inset>Лабораторные у которых прошел дедлайн</v-subheader>
            <v-list-item
                :value="sol.lab.id"
                v-for="sol in $store.state.solutions.finishedSolutions"
                :key="sol.lab.id"
                @click="goToLab(sol)">
              <template v-slot:default="{ active }">
                <v-list-item-avatar color="primary" >
                  <v-icon  dark >mdi-clipboard-text</v-icon>
                </v-list-item-avatar>
                <v-list-item-content>
                  <v-list-item-title>{{sol.lab.title}}</v-list-item-title>
                  <v-list-item-subtitle>{{getDate(sol.lab.deadline)}}</v-list-item-subtitle>
                </v-list-item-content>

                <v-list-item-icon v-if="sol.grade != null">
                  {{sol.grade}} / {{sol.lab.maxGrade}}
                  <v-icon color="primary">
                    mdi-star
                  </v-icon>
                </v-list-item-icon>

                <v-list-item-icon>
                  <div :class="''+getStatusStyle(sol)">{{getStatus(sol)}}</div>
                </v-list-item-icon>

              </template>

            </v-list-item>


          </v-list-item-group>
        </v-list>
      </v-card>
    </div>



  </div>
</template>

<script>
import Swal from "sweetalert2";
import moment from "moment";
export default {
  name: "StudentLabSolutions",
  data(){
    return{
      items:[],
      labs:null,
      elevation:0,
      titleRules: [
        v => !!v || 'Код класса не указан'
      ],
      isEditing: false,
      model: null,

      classCode:null
    }
  },

  destroyed () {
    window.removeEventListener('scroll', this.handleScroll);
  },

  async created() {
    window.addEventListener('scroll', this.handleScroll);
    await this.$store.dispatch("GetSolutionsStudent")
    console.log(this.$store.state.solutions)
    this.getItems()
  },

  methods:{
    getItems(){
      var result =[]

      this.$store.state.solutions.activeSolutions.forEach((s) =>{
        result.push( {text:s.lab.title,value:s})
      })

      this.$store.state.solutions.finishedSolutions.forEach((s) =>{
        result.push( {text:s.lab.title, value:s})
      })

      this.items = result;

    },

    goToLab(sol) {
      this.$router.push('/student/class/'+sol.lab.classRoomId+'/lab/'+sol.lab.id+'/student/'+this.$store.state.user.id)
    },

    getStatusStyle(sol){
      if(sol.status == 0 && sol.solution ==null){
        return "blue--text"
      }
      if(sol.status == 0 && sol.solution !=null){
        return "primary--text"
      }
      if(sol.status == 1){
        return "green--text"
      }
      if(sol.status == 2){
        return "red--text"
      }
    },

    getStatus(sol){
      if(sol.status == 0 && sol.solution ==null){
        return "Назначено"
      }
      if(sol.status == 0 && sol.solution !=null){
        return "Сдано"
      }
      if(sol.status == 1){
        return "Проверено"
      }
      if(sol.status == 2){
        return "Отклонено"
      }
    },

    getDate(date){
      return moment(date).format('ll');
    },

    handleScroll (event) {
      if(window.scrollY > 0){
        this.elevation = 5
      }else {
        this.elevation = 0
      }
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