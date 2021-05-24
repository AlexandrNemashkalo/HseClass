<template>
  <div>

    <v-btn v-if="tab==0" fixed style="z-index: 100;bottom: 20px;right: 20px"  x-large
           @click="fab = !fab; labs=[]"
           v-model="fab" color="primary" dark fab
    >
      <v-icon large v-if="fab">
        mdi-close
      </v-icon>

      <v-icon  large v-if="!fab  ">
        mdi-clipboard-edit-outline
      </v-icon>
    </v-btn>

    <v-toolbar elevation="0" v-show="fab" style="z-index: 100;bottom: 100px;right: 20px; position: fixed; background-color:rgba(0,0,0,0)"  extended>
    <template v-slot:extension>


      <v-dialog
          v-model="dialogAdd"
          persistent
          max-width="500px"
      >
        <template v-slot:activator="{ on, attrs }">

          <v-fab-transition>
            <v-btn v-bind="attrs"  v-on="on" absolute v-if="fab" @click="isAdd=true"  fab dark small color="indigo">
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </v-fab-transition>

        </template>
        <v-card>
          <v-card-title>
            <span class="headline">Назначение лабораторной классу</span>
          </v-card-title>
          <v-card-text>
            <v-form ref="formAdd"
                    v-model="validAdd"
                    lazy-validation>
              <v-text-field
                  label="Название класса"
                  v-model="formAdd.title"
                  :rules="titleRules"
                  required
              ></v-text-field>

              <v-select
                  :rules="taskRules"
                  v-model="formAdd.taskLabId"
                  :items="tasks"
                  label="Standard"
              ></v-select>

              <v-text-field
                  :rules="gradeRules"
                  label="Максимальный балл"
                  v-model="formAdd.maxGrade"
                  required
              ></v-text-field>

              <v-menu
                  ref="menu"
                  v-model="menu"
                  :close-on-content-click="false"
                  :return-value.sync="date"
                  transition="scale-transition"
                  offset-y
                  min-width="auto"
              >
                <template v-slot:activator="{ on, attrs }">
                  <v-text-field
                      :rules="dateRules"
                      v-model="date"
                      label="Дедлайн работы"
                      prepend-icon="mdi-calendar"
                      readonly
                      v-bind="attrs"
                      v-on="on"
                  ></v-text-field>
                </template>
                <v-date-picker
                    max="2022-03-20"
                    :allowed-dates="allowedDates"
                    v-model="date"
                    no-title
                    scrollable
                >
                  <v-spacer></v-spacer>
                  <v-btn
                      text
                      color="primary"
                      @click="menu = false"
                  >
                    Отменить
                  </v-btn>
                  <v-btn
                      text
                      color="primary"
                      @click="$refs.menu.save(date)"
                  >
                    ОК
                  </v-btn>
                </v-date-picker>
              </v-menu>
            </v-form>
          </v-card-text>


          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
                color="blue darken-1"
                text
                @click="dialogAdd = false"
            >
              Закрыть
            </v-btn>
            <v-btn
                color="blue darken-1"
                text
                @click="addLab()"
            >
              Сохранить
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>


      <v-dialog
          v-model="dialogEdit"
          persistent
          max-width="500px"
      >
        <template v-slot:activator="{ on, attrs }">

          <v-fab-transition>
            <v-btn  v-bind="attrs"
                    v-on="on"  absolute v-if="fab"  :disabled="labs.length!=1" style="bottom: 55px;" @click="showEdit()" fab dark small color="green">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
          </v-fab-transition>

        </template>
        <v-card>
          <v-card-title>
            <span class="headline">Редактирование лабораторной работы</span>
          </v-card-title>
          <v-card-text>
            <v-form ref="formEdit"
                    v-model="validEdit"
                    lazy-validation>
              <v-text-field
                  label="Название класса"
                  v-model="formEdit.title"
                  :rules="titleRules"
                  required
              ></v-text-field>

              <v-select
                  :rules="taskRules"
                  v-model="formEdit.taskLabId"
                  :items="tasks"
                  label="Standard"
              ></v-select>

              <v-text-field
                  :rules="gradeRules"
                  label="Максимальный балл"
                  v-model="formEdit.maxGrade"
                  required
              ></v-text-field>

              <v-menu
                  ref="menu"
                  v-model="menu"
                  :close-on-content-click="false"
                  :return-value.sync="date"
                  transition="scale-transition"
                  offset-y
                  min-width="auto"
              >
                <template v-slot:activator="{ on, attrs }">
                  <v-text-field
                      :rules="dateRules"
                      v-model="date"
                      label="Дедлайн работы"
                      prepend-icon="mdi-calendar"
                      readonly
                      v-bind="attrs"
                      v-on="on"
                  ></v-text-field>
                </template>
                <v-date-picker
                    max="2022-03-20"
                    :allowed-dates="allowedDates"
                    v-model="date"
                    no-title
                    scrollable
                >
                  <v-spacer></v-spacer>
                  <v-btn
                      text
                      color="primary"
                      @click="menu = false"
                  >
                    Отменить
                  </v-btn>
                  <v-btn
                      text
                      color="primary"
                      @click="$refs.menu.save(date)"
                  >
                    ОК
                  </v-btn>
                </v-date-picker>
              </v-menu>


            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
                color="blue darken-1"
                text
                @click="dialogEdit = false"
            >
              Закрыть
            </v-btn>
            <v-btn
                color="blue darken-1"
                text
                @click="editLab()"
            >
              Сохранить
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>


      <v-fab-transition>
      <v-btn v-if="fab"  :disabled="labs.length==0"  @click="deleteLabs()" style="bottom: 100px;" fab dark small color="red">
        <v-icon>mdi-delete</v-icon>
      </v-btn>
      </v-fab-transition>
    </template>
    </v-toolbar>


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
                  multiple
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
                  <v-list-item-action class="mr-15" v-if="fab">
                    <v-checkbox
                        :input-value="active"
                        color="primary"
                    ></v-checkbox>
                  </v-list-item-action>
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
                  <v-list-item-action class="mr-15" v-if="fab">
                    <v-checkbox
                        :input-value="active"
                        color="primary"
                    ></v-checkbox>
                  </v-list-item-action>
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
  name: "TeacherClassInfo",
  data(){
    return{

      date: null,
      menu: false,

      validAdd: true,
      validEdit: true,
      dialogEdit:false,
      dialogAdd:false,
      isDelete:false,
      isEdit:false,
      isAdd:false,

      fab: false,

      formAdd:{
        title:"",
        taskLabId: 0,
        classRoomId: 0,
        maxGrade: 0,
        deadline: moment()
      },
      taskRules:[
        v => !!v || 'Задание не выбрано'
      ],
      titleRules: [
        v => !!v || 'Название лабораторной не указано'
      ],
      gradeRules: [
        v => !!v || 'Максимальный бал не указан' ,
         v => /^-?\d+$/.test(v) || 'Значение не является числом'
      ],
      dateRules: [
        v => !!v || 'Дедлайн не указан'
      ],

      formEdit:{
        title:"",
        taskLabId: 0,
        classRoomId: 0,
        maxGrade: 0,
        deadline: "2021-05-23T19:29:32.955Z"
      },

      teacher: {
        name:"",
        id:0,
        email:""
      },
      tab: null,
      labs: [],
      tasks:[]
    }
  },

  async created() {
      await this.$store.dispatch("GetClassInfoTeacher", this.$route.params.classId).then(()=>{
          this.$store.dispatch("GetTasksTeacher")
          this.tasks = this.$store.state.tasks.map(t => {return { text:t.name, value:t.id  }})
      })
       this.teacher = this.$store.state.classInfo.users.filter(u => u.isTeacher)[0]
  },

  methods:{
    allowedDates(val){
      return  moment(val).isAfter(moment())
    },

    showEdit(){
      var lab = this.$store.state.classInfo.labs.filter(l => {return l.id == this.labs[0]})[0]
      console.log(lab)
      this.formEdit.taskLabId = lab.taskLabId
      this.formEdit.id = lab.id
      this.formEdit.deadline = lab.deadline
      this.formEdit.maxGrade = lab.maxGrade
      this.formEdit.title = lab.title
      this.formEdit.classRoomId = lab.classRoomId
      this.date =  lab.deadline
    },

    async addLab(){
      if(this.$refs.formAdd.validate()){
        var lab ={
          taskLabId : this.formAdd.taskLabId,
          deadline:this.date,
          maxGrade:this.formAdd.maxGrade,
          title: this.formAdd.title,
          classRoomId:this.$store.state.classInfo.id
        }
        await this.$store.dispatch("AddLabTeacher",lab).then( ()=>{
          this.dialogAdd = false;
        })
      }
    },

    async editLab(){
      if(this.$refs.formEdit.validate()){
        var lab ={
          taskLabId : this.formEdit.taskLabId,
          deadline:this.date,
          maxGrade:this.formEdit.maxGrade,
          title: this.formEdit.title,
          classRoomId:this.formEdit.classRoomId
        }
        await this.$store.dispatch("EditLabTeacher",{id:this.formEdit.id,data:lab}).then( ()=>{
          this.dialogEdit = false;
        })
      }
    },

    async deleteLabs(){
      await Swal.fire({
        title: 'Вы действительно хотите удалить все выбранные работы?',
        text: "После удаление вы больше не сможете их восстановить",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Отменить',
        confirmButtonText: 'Да, удалить'
      }).then( async  (result) => {
        if (result.isConfirmed) {
          for (const l of this.labs) {
            await this.$store.dispatch("DeleteLabTeacher",l);
          }
          await this.$store.dispatch("GetClassInfoTeacher", this.$store.state.classInfo.id)
                Swal.fire(
                'Удалено!',
                'Лабораторные были успешно удалены',
                'success')
        }
      })
    },

    goToLab(id) {
      if(!this.fab){
        this.$router.push('/teacher/class/'+this.$store.state.classInfo.id+'/lab/'+id)
      }

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

<style scoped>

</style>