import mutations from './mutations'
import { parseJwt} from "./func"
import authModule from "./modules/authModule";
import teacherModule from "./modules/teacherModule";
import studentModule from "./modules/studentModule";
import router from '../router'
import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersist from 'vuex-persist';
import Swal from "sweetalert2";


import axios from 'axios'

Vue.use(Vuex)

const vuexLocalStorage = new VuexPersist({
  key: 'vuex',
  storage: window.localStorage,
})


export default new Vuex.Store({
  state: {
    isDark:false,
    port: "https://hse-class.ru/api/",  // https://localhost:5001/api/
    token:null,
    windowWidth:null,
    user:{
      id:null,
      name:null,
      email:null,
      isTeacher:null
    },
    classInfo:null,
    labInfo:null,
    solutionInfo:null,
    classes:null,
    solutions:null,
    tasks:null
  },
  mutations,
  modules: {
    authModule,
    teacherModule,
    studentModule
  },
  actions: {

    Exit({commit}){
      commit("setUser",null)
      commit("setToken",null)
      commit("setClasses",null)
      commit("setSolutions",null)
      commit("setTasks",null)
      commit("setLabInfo",null)
      commit("setClassInfo",null)
      commit("setSolutionInfo",null)
    },

    CheckUser(){
      return this.state.token != null
          && this.state.user != null
          && this.state.user.id != null
          && this.state.user.email != null
    },

    CheckStudent(){
      return this.dispatch("CheckUser")
          && !this.state.user.isTeacher
    },

    CheckTeacher(){
      return this.dispatch("CheckUser")
          && this.state.user.isTeacher
    },

    async GetUserInfo(){
      var config ={headers:{ Authorization :"Bearer "+ this.state.token}}
      await axios.get(this.state.port +'user',config).then(response =>{
        console.log(response)
        this.commit('setUser',response.data)
      }).catch(function (error) {
        Swal.fire({
          icon: 'error',
          title: 'Ошибка',
        })
        console.log(error)});
    },

  },
  plugins: [vuexLocalStorage.plugin]
})
