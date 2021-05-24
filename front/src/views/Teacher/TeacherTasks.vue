<template>
<div>

  <v-card class="pl-2 pr-2"   style="position: fixed; width:100%;margin:-16px;z-index: 100; border-radius: 0px" :elevation="elevation">

    <v-card-actions style="text-align: right;width: 100%">

      <div>
        <span style="font-size: 25px">Задания</span>
      </div>
      <div class="search">

        <v-combobox
            color="primary"
            class="mr-1 ml-5 searchInput"
            v-model='model'
            :items="$store.state.tasks.map((c) =>{return {text:c.name,value:c.id}})"
            label="Поиск по названию"
            clearable
            :auto-select-first='true'
            v-on:change='onChange'
        ></v-combobox>
      </div>
    </v-card-actions>
  </v-card >



<div style="padding-top: 80px;">

  <v-card
      v-for="task in $store.state.tasks" :key="task.id"
      v-if="searchId == null || (searchId == task.id)"
      class="mx-auto w-100 mb-5"
      elevation="4"
  >
    <v-card-text>
      <div>{{task.theme}}</div>
      <p class="display-1 primary--text">
        {{task.name}}
      </p>
      <p>Оборудование: {{task.equipment}}</p>
      <div class="text--primary"> {{task.description}}</div>

    </v-card-text>
    <v-card-actions>
      <v-btn
          text
          color="primary"
          @click="goToManual(task.linkToManual)"
      >
        Перейти к методичке
      </v-btn>
    </v-card-actions>

    <v-expand-transition>
      <v-card
          v-if="reveal"
          class="transition-fast-in-fast-out v-card--reveal"
          style="height: 100%;"
      >
        <v-card-text class="pb-0">
          <p class="display-1 text--primary">
            Origin
          </p>
          <p>late 16th century (as a noun denoting a place where alms were distributed): from medieval Latin eleemosynarius, from late Latin eleemosyna ‘alms’, from Greek eleēmosunē ‘compassion’ </p>
        </v-card-text>
        <v-card-actions class="pt-0">
          <v-btn
              text
              color="teal accent-4"
              @click="reveal = false"
          >
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-expand-transition>
  </v-card>

</div>


</div>
</template>

<script>
export default {
  name: "TeacherTasks",
  data(){
    return{
      reveal: false,
      searchId:null,
      elevation:0,

      model: null,
    }
  },
  destroyed () {
    window.removeEventListener('scroll', this.handleScroll);
  },

   created(){
    window.addEventListener('scroll', this.handleScroll);
    this.$store.dispatch("GetTasksTeacher");
  },

  methods:{
    onChange: function (e){
      if(e != null)
      this.searchId = e.value;
      else
        this.searchId = null
    },

    goToManual(link){
      document.location.href = link;
    },
    handleScroll (event) {
      if(window.scrollY > 0){
        this.elevation = 5
      }else {
        this.elevation = 0
      }
    },
  }
}
</script>

<style scoped>

</style>