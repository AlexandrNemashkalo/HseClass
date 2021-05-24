<template>
  <div>

    <v-card
        class="mx-auto w-100"
    >
      <v-card-text>
        <p class="display-1 primary--text">
          {{ $store.state.labInfo.title }}
        </p>
        <p>Дедлайн: {{$store.state.labInfo.deadline}} <br> Max балл: {{$store.state.labInfo.maxGrade}} </p>
        <div class="text--primary">
          Задание: {{$store.state.labInfo.task.name}} <br>
          Описание: {{$store.state.labInfo.task.description}} <br>
          Правильное решение: {{$store.state.labInfo.task.correctSolution}} <br>
          <a class="primary--text" @click="goToManual($store.state.labInfo.task.linkToManual)">
            Методические указания
          </a>

        </div>

      </v-card-text>

      <v-tabs
          v-model="tab"
          background-color="transparent"
          color="basil"
          grow
      >
        <v-tab class="myTab">
          Без решения
        </v-tab>
        <v-tab  class="myTab">
        С решением
        </v-tab>
        <v-tab  class="myTab">
          Проверено
        </v-tab>
        <v-tab  class="myTab">
          Отклонено
        </v-tab>
      </v-tabs>


      <v-tabs-items v-model="tab">
        <v-tab-item>
          <v-card color="basil" flat>
            <v-list two-line>
              <template v-if="sol.solution == null && sol.status ==0" v-for="(sol, index) in $store.state.labInfo.solutions">
                <v-list-item :key="sol.userId">
                  <v-list-item-avatar large color="orange">{{sol.userName.toUpperCase()[0]}}</v-list-item-avatar>
                  <v-list-item-content>
                    <v-list-item-title>{{sol.userName}}</v-list-item-title>
                    <v-list-item-subtitle >{{sol.userEmail}}</v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
              </template>
            </v-list>
          </v-card>
        </v-tab-item>



        <v-tab-item>
          <v-card color="basil" flat>
            <v-list two-line>
              <v-list-item-group
                  v-model="selectedItem"
                  color="primary"
              >
              <template v-if="sol.solution != null && sol.status ==0" v-for="(sol, index) in $store.state.labInfo.solutions">
                <v-list-item @click="showEdit(sol)"   :key="sol.userId">
                  <v-list-item-avatar large color="orange">{{sol.userName.toUpperCase()[0]}}</v-list-item-avatar>
                  <v-list-item-content>
                    <v-list-item-title>{{sol.userName}}</v-list-item-title>
                    <v-list-item-subtitle >{{sol.userEmail}} <br> {{sol.dateOfDownload}} </v-list-item-subtitle>

                  </v-list-item-content>
                </v-list-item>
              </template>
              </v-list-item-group>
            </v-list>
          </v-card>
        </v-tab-item>

        <v-tab-item>
          <v-card color="basil" flat>
            <v-list two-line>
              <v-list-item-group
                  v-model="selectedItem"
                  color="primary"
              >
                <template v-if="sol.solution != null && sol.status ==1" v-for="(sol, index) in $store.state.labInfo.solutions">
                  <v-list-item @click="showEdit(sol)" :key="sol.userId">
                    <v-list-item-avatar large color="orange">{{sol.userName.toUpperCase()[0]}}</v-list-item-avatar>
                    <v-list-item-content>
                      <v-list-item-title>{{sol.userName}}</v-list-item-title>
                      <v-list-item-subtitle >{{sol.userEmail}} <br> {{sol.dateOfDownload}} </v-list-item-subtitle>

                    </v-list-item-content>
                    <v-list-item-icon>
                      {{sol.grade}} / {{$store.state.labInfo.maxGrade}}
                      <v-icon color="primary">
                        mdi-star
                      </v-icon>
                    </v-list-item-icon>
                  </v-list-item>
                </template>
              </v-list-item-group>
            </v-list>
          </v-card>
        </v-tab-item>


        <v-tab-item>
          <v-card color="basil" flat>
            <v-list two-line>
              <v-list-item-group
                  v-model="selectedItem"
                  color="primary"
              >
                <template v-if="sol.solution != null && sol.status ==2" v-for="(sol, index) in $store.state.labInfo.solutions">
                  <v-list-item @click="showEdit(sol)" :key="sol.userId">
                    <v-list-item-avatar large color="orange">{{sol.userName.toUpperCase()[0]}}</v-list-item-avatar>
                    <v-list-item-content>
                      <v-list-item-title>{{sol.userName}}</v-list-item-title>
                      <v-list-item-subtitle >{{sol.userEmail}} <br> {{sol.dateOfDownload}} </v-list-item-subtitle>

                    </v-list-item-content>
                  </v-list-item>
                </template>
              </v-list-item-group>
            </v-list>
          </v-card>
        </v-tab-item>
      </v-tabs-items>

    </v-card>


    <v-dialog
        v-model="isEdit"
        max-width="500"
        style="z-index: 2000"
        persistent
    >
      <v-card>

        <v-card-title>
          <span class="headline">Проверка решения</span>
        </v-card-title>
        <v-card-text>


          <p>Студент: {{editedForm.userName}} <br> Email: {{editedForm.userEmail}} </p>
          <p>Решение: {{editedForm.solution}} <br>
          Дата загрузки: {{editedForm.dateOfDownload}} <br>
          <span v-if="editedForm.timeSpan != null">Время выполнения: {{editedForm.timeSpan}} <br></span>
          <a class="primary--еуче" :href="editedForm.videoPath" v-if="editedForm.videoPath !=null" target="_blank">Видео выполнения</a>
          </p>

          <v-form ref="formEdit"
                  v-model="validEdit"
                  lazy-validation>
            <v-text-field
                label="Оценка"
                :rules="gradeRules"
                v-model="editedForm.grade"
                required
            ></v-text-field>
          </v-form>


        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
              color="blue darken-1"
              text
              @click="isEdit = false;"
          >
            Закрыть
          </v-btn>
          <v-btn
              color="red"
              text
              @click="declineSol()"
          >
            Отклонить
          </v-btn>
          <v-btn
              color="green"
              text
              @click="estimateSol()"
          >
            Оценить
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>


  </div>
</template>

<script>
export default {
  name: "TeacherLabInfo",
  data(){
    return{
      reveal: false,
      task:null,
      tab: null,
      selectedItem: null,

      gradeRules: [
        v => !!v || 'Максимальный бал не указан' ,
        v => /^-?\d+$/.test(v) || 'Значение не является числом',
        v => parseInt(v) <= this.$store.state.labInfo.maxGrade || 'Нельзя поставить оценку выше макимальной',
      ],

      elevation:0,
      valid: true,
      validEdit: true,

      isEditing: false,
      model: null,
      dialog: false,
      isEdit: false,

      editedForm:{
        dateOfDownload:"",
        grade:0,
        labId:null,
        solution:null,
        status:null,
        timeSpan:null,
        userEmail:null,
        userName:null,
        userId:null,
        videoPath:null
      }

    }
  },

  async created(){
    await this.$store.dispatch("GetLabInfoTeacher", this.$route.params.labId)
    console.log(this.$store.state.labInfo)
  },
  methods:{
    showEdit(sol){
      this.editedForm={
            dateOfDownload:sol.dateOfDownload,
            grade:sol.grade,
            labId:sol.labId,
            solution:sol.solution,
            status:sol.status,
            timeSpan:sol.timeSpan,
            userEmail:sol.userEmail,
            userName:sol.userName,
            userId:sol.userId,
            videoPath:sol.videoPath
      }

      this.isEdit = true;
    },

    async editSol(){
        await this.$store.dispatch("EditSolutionTeacher",this.editedForm)
        this.isEdit = false;
    },

    async estimateSol(){
      if(this.$refs.formEdit.validate()) {
        this.editedForm.status = 1;
        await this.editSol();
      }
    },

    async declineSol(){
      this.editedForm.grade = null;
      this.editedForm.status = 2;
      await this.editSol();
    },

    goToManual(link){
      document.location.href = link;
    },
  }
}
</script>

<style scoped>

</style>