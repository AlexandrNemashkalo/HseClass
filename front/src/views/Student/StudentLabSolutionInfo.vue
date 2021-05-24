<template>
  <div>

    <v-btn
        v-if="$store.state.labInfo.solution.solution != null"
        @click="showEdit($store.state.labInfo.solution)"
        x-large
        fab
        style="position:fixed;bottom: 30px;right: 30px;z-index: 200"
        color="primary"
        elevation="4"
    >
      <v-icon x-large>mdi-pencil</v-icon>
    </v-btn>

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

          <a class="primary--text" @click="goToManual($store.state.labInfo.task.linkToManual)">
            Методические указания
          </a>

        </div>

      </v-card-text>

    </v-card>

    <v-card
        class="mx-auto w-100 mt-5"
    >
      <v-card-text v-if="$store.state.labInfo.solution.solution != null">
        <div :class="getStatusStyle($store.state.labInfo.solution)" style="position: absolute;right: 20px;font-size: 14px">
          {{getStatus($store.state.labInfo.solution)}}
        </div>
        <p class="display-1 primary--text">Решение</p>
        <p>Дата загрузки: {{$store.state.labInfo.solution.dateOfDownload}} <br>
          <span v-if="$store.state.labInfo.solution.timeSpan != null">
            Время выполнения: {{$store.state.labInfo.solution.timeSpan}} <br>
          </span>
          <span v-if="$store.state.labInfo.solution.grade!= null">
            Оценка: {{$store.state.labInfo.solution.grade}}
          </span>
        </p>

        <p>Решение: {{$store.state.labInfo.solution.solution}} <br>

        </p>


        <div v-if="$store.state.labInfo.solution.videoPath != null">
        <LazyYoutube
            ref="youtubeLazyVideo"
            :src="$store.state.labInfo.solution.videoPath"
            max-width="720px"
            aspect-ratio="16:9"
            thumbnail-quality="standard"
        />
        </div>


      </v-card-text>

      <v-card-text style="text-align: center" v-if="$store.state.labInfo.solution.solution == null">

        <p class="display-1 primary--text">Решение отсутствует</p>
        <v-btn @click="showEdit($store.state.labInfo.solution)">
          Открыть форму
        </v-btn>

      </v-card-text>

    </v-card>


    <v-dialog
        v-model="isEdit"
        max-width="500"
        style="z-index: 2000"
        persistent
    >
      <v-card>

        <v-card-title>
          <span class="headline">Сдать решение</span>
        </v-card-title>
        <v-card-text>

          <v-form ref="form"
                  v-model="validEdit"
                  lazy-validation>
            <v-text-field
                label="Решение"
                v-model="editedForm.solution"
                :rules="solutionRules"
                required
            ></v-text-field>
            <v-text-field
                label="Ссылка на видео"
                v-model="editedForm.videoPath"
            ></v-text-field>
            <v-text-field
                label="Время выполнения"
                v-model="editedForm.timeSpan"
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
              color="green"
              text
              @click="editSol()"
          >
            Сдать
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>


  </div>
</template>

<script>

import { LazyYoutube, LazyVimeo } from "vue-lazytube";

export default {
  name: "TeacherLabInfo",
  components: {
    LazyYoutube,
    LazyVimeo
  },
  data(){
    return{
      reveal: false,
      task:null,

      solutionRules: [
        v => !!v || 'Решение не указано' ,
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
      },

      youtubeLink:'https://www.youtube.com/watch?v=TcMBFSGVi1c',
      vimeoLink:'https://player.vimeo.com/video/64654583'

    }
  },

  async created(){
    await this.$store.dispatch("GetLabInfoStudent", this.$route.params.labId)

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
      if(this.$refs.form.validate()){
        await this.$store.dispatch("EditSolutionStudent",this.editedForm)
        this.isEdit = false;
      }
    },


    goToManual(link){
      document.location.href = link;
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
  }
}
</script>
