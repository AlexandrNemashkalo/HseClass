import axios from "axios";
import store from "@/store/index";
import router from "../../router";
import Swal from "sweetalert2";

export default {
    actions: {
        async RegisterStudent(state,data){
            store.dispatch("Exit");
            await axios.post(store.state.port +'auth/registerStudent',data).then(async response =>{
                store.commit('setToken',response.data)
                await store.dispatch("GetUserInfo")
                router.replace('/student/')
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Не удалось зарегистрироваться',
                })
                console.log(error)});
        },

        async RegisterTeacher(state,data){
            store.dispatch("Exit");
            await axios.post(store.state.port +'auth/registerTeacher',data).then(async response =>{
                store.commit('setToken',response.data)
                await store.dispatch("GetUserInfo")
                router.replace('/teacher/')
            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Не удалось зарегистрироваться',
                })
                console.log(error)});
        },

        async Login(state,data){
            await axios.post(store.state.port +'auth/login',data).then(async response =>{
                store.commit('setToken',response.data)
                await store.dispatch("GetUserInfo")

                if(store.state.user.isTeacher){
                    router.replace('/teacher/')
                }
                else{
                    router.replace('/student/')
                }

            }).catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Не удалось авторизоваться',
                })
                console.log(error)});
        },
    },
}