<template>
  <div>

    <v-card
        class="mx-auto"
        style="position: relative"
    >


      <v-img
          class="white--text align-end"
          height="200px"
          :src="require('@/assets/imgs/class'+ Math.round(($store.state.classInfo.id % 5))+'.jpg')"
      >
        <v-card-title  style="font-size: 30px;">{{$store.state.classInfo.title}}</v-card-title>
      </v-img>


      <v-tabs
          v-model="tab"
          background-color="transparent"
          color="basil"
          grow
      >
        <v-tab  class="myTab">
          Лабораторные работы
        </v-tab>
        <v-tab class="myTab">
          Пользователи
        </v-tab>
      </v-tabs>

      <v-tabs-items v-model="tab">
        <v-tab-item>
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
                    :value="lab.id"
                    v-if="isActiveLab(lab.deadline)"
                    v-for="lab in $store.state.classInfo.labs"
                    :key="lab.id"
                    @click="goToLab(lab.id)">
                  <template v-slot:default="{ active }">
                    <v-list-item-avatar color="primary" >
                      <v-icon  dark >mdi-clipboard-text</v-icon>
                    </v-list-item-avatar>
                    <v-list-item-content>
                      <v-list-item-title>{{lab.title}}</v-list-item-title>
                      <v-list-item-subtitle>{{getDate(lab.deadline)}}</v-list-item-subtitle>
                    </v-list-item-content>

                    <v-list-item-icon v-if="lab.mySolution.grade != null">
                      {{lab.mySolution.grade}} / {{lab.maxGrade}}
                      <v-icon color="primary">
                        mdi-star
                      </v-icon>
                    </v-list-item-icon>

                    <v-list-item-icon>
                        <div :class="''+getStatusStyle(lab.mySolution)">{{getStatus(lab.mySolution)}}</div>
                    </v-list-item-icon>

                  </template>

                </v-list-item>

                <v-divider></v-divider>
                <v-subheader inset>Лабораторные у которых прошел дедлайн</v-subheader>
                <v-list-item
                    v-if="!isActiveLab(lab.deadline)"
                    v-for="lab in $store.state.classInfo.labs"
                    :key="lab.id"
                    @click="goToLab(lab.id)"
                >
                  <template v-slot:default="{ active }">
                    <v-list-item-avatar color="primary">
                      <v-icon dark >mdi-clipboard-text</v-icon>
                    </v-list-item-avatar>
                    <v-list-item-content>
                      <v-list-item-title v-text="lab.title"></v-list-item-title>
                      <v-list-item-subtitle>{{getDate(lab.deadline)}}</v-list-item-subtitle>
                    </v-list-item-content>
                    <v-list-item-icon>
<!--                      {{sol.grade}} / {{$store.state.labInfo.maxGrade}}-->
                      <v-icon color="primary">
                        mdi-star
                      </v-icon>
                    </v-list-item-icon>
                  </template>
                </v-list-item>


              </v-list-item-group>
            </v-list>
          </v-card>
        </v-tab-item>



        <v-tab-item>
          <v-card color="basil" flat>
            <v-list two-line>

              <v-subheader>Учитель</v-subheader>

              <v-list-item >
                <v-list-item-avatar large color="orange">{{teacher.name.toUpperCase()[0]}}</v-list-item-avatar>
                <v-list-item-content>
                  <v-list-item-title >{{teacher.name}}</v-list-item-title>
                  <v-list-item-subtitle >{{teacher.email}}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>

              <v-divider></v-divider>

              <v-subheader>Студенты</v-subheader>

              <template v-for="(user, index) in $store.state.classInfo.users">
                <v-list-item  v-if="!user.isTeacher" :key="user.id">
                  <v-list-item-avatar large color="orange">{{user.name.toUpperCase()[0]}}</v-list-item-avatar>
                  <v-list-item-content>
                    <v-list-item-title>{{user.name}}</v-list-item-title>
                    <v-list-item-subtitle >{{user.email}}</v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
              </template>
            </v-list>
          </v-card>
        </v-tab-item>
      </v-tabs-items>


    </v-card>


  </div>
</template>

<script>

import moment from 'moment'
import Swal from "sweetalert2";
export default {
  name: "StudentClassInfo",
  data(){
    return{

      teacher: {
        name:"",
        id:0,
        email:""
      },
      tab: null,
      labs: []
    }
  },

  async created() {
    await this.$store.dispatch("GetClassInfoStudent", this.$route.params.classId)
    this.teacher = this.$store.state.classInfo.users.filter(u => u.isTeacher)[0]
  },

  methods:{

    getStatusStyle(sol){
    console.log(this.$store.state.classInfo)
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
      console.log(this.$store.state.classInfo)
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

    allowedDates(val){
      return  moment(val).isAfter(moment())
    },


    goToLab(id) {
      this.$router.push('/student/class/'+this.$route.params.classId+'/lab/'+id+'/student/'+ this.$store.state.user.id)
    },

    getDate(date){
      return moment(date).format('ll');
    },

    isActiveLab(date){
      return moment(date).isAfter(moment());
    }
  }
}
</script>

