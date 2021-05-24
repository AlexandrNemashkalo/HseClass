import axios from "axios";
import router from "../../router";
import store from "@/store/index";
import Swal from "sweetalert2";

export default {
    actions: {

        async GetClassesStudent(state){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'student/class', config).then(async response =>{
                store.commit('setClasses',response.data)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async  JoinToClassStudent(state,code){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.put(store.state.port +'student/class/'+code,{}, config).then(async response =>{
                store.dispatch("GetClassesStudent")
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                    text:'Класс с таким кодом не существует или вы уже состоите в нем'
                })
                console.log(error)});
        },

        async GetClassInfoStudent(state,id){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'student/class/'+id, config).then(async response =>{
                store.commit("setClassInfo", response.data)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async GetLabInfoStudent(state, id){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'student/lab/'+id, config).then( response =>{
                store.commit("setLabInfo", response.data)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },


        async EditSolutionStudent(state, form){
            var data ={
                solution:form.solution,
                timeSpan:form.timeSpan,
                video:form.videoPath
            }
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.put(store.state.port +'student/solution/'+form.labId, data, config).then(async response =>{
                store.dispatch("GetLabInfoStudent",form.labId)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async GetSolutionsStudent(state){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'student/solution', config).then(async response =>{
                store.commit("setSolutions", response.data)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },



    },
}