import axios from "axios";
import router from "../../router";
import store from "@/store/index";
import Swal from "sweetalert2";

export default {
    actions: {

        async GetTasksTeacher(state){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'teacher/taskLab', config).then( response =>{
                console.log(response)
                store.commit('setTasks',response.data.tasks)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async GetClassesTeacher(state){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'teacher/class', config).then(async response =>{
                store.commit('setClasses',response.data)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async AddClassTeacher(state, data){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.post(store.state.port +'teacher/class',data, config).then(async response =>{
                await store.dispatch("GetClassesTeacher")
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async EditClassTeacher(state, form){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.put(store.state.port +'teacher/class/'+form.id, form.data, config).then(async response =>{
                await store.dispatch("GetClassesTeacher")
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async GetClassInfoTeacher(state,id){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'teacher/class/'+id, config).then(async response =>{
                store.commit("setClassInfo", response.data)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async DeleteClassTeacher(state,id){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.delete(store.state.port +'teacher/class/'+id, config).then(async response =>{
                if(response.data){
                   await store.dispatch("GetClassesTeacher")
                }
                else{
                    throw 'error'
                }
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});

        },

        async DeleteLabTeacher(state,id){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.delete(store.state.port +'teacher/lab/'+id, config).then(async response =>{
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async EditLabTeacher(state,form){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.put(store.state.port +'teacher/lab/'+form.id, form.data, config).then(async response =>{
                store.dispatch("GetClassInfoTeacher",form.data.classRoomId)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async AddLabTeacher(state, data){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.post(store.state.port +'teacher/lab', data, config).then(async response =>{
                store.dispatch("GetClassInfoTeacher",data.classRoomId)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async GetLabInfoTeacher(state, id){
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.get(store.state.port +'teacher/lab/'+id+'/user', config).then(async response =>{
                store.commit("setLabInfo", response.data)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },

        async EditSolutionTeacher(state, form){
            var data ={
                grade:form.grade,
                status:form.status
            }
            var config ={headers:{ Authorization :"Bearer "+ store.state.token}}
            await axios.put(store.state.port +'teacher/lab/'+form.labId +'/user/'+form.userId,data, config).then(async response =>{
                store.dispatch("GetLabInfoTeacher",form.labId)
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ошибка',
                })
                console.log(error)});
        },


    },
}