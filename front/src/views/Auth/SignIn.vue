<template>
  <div style="height: 100vh;" >

      <v-card
          class="signinContainer"
          elevation="2"
      >

        <v-card
            elevation="0"
            class="formCard"
        >
          <v-card-title>Авторизация</v-card-title>

          <v-form
              ref="form"
              v-model="valid"
              lazy-validation
          >
            <v-text-field
                v-model="email"
                :rules="emailRules"
                label="E-mail"
                required
            ></v-text-field>

            <v-text-field
                v-model="password"
                :rules="passwordRules"
                label="Пароль"
                required
            ></v-text-field>

            <v-btn

                :disabled="!valid"
                color="primary"
                class="mr-4 mt-4 mb-4 w-100"
                @click="validate"
            >Войти</v-btn>

            <span style="font-size: 12px">Нет аккаунта?
              <a @click="$router.push('register-teacher')" class="primary--text">Зарегистрироваться как учитель</a>
              <br> <a @click="$router.push('register-student')" class="primary--text">Зарегистрироваться как студент</a> </span>
          </v-form>


        </v-card>
      </v-card>


    <img style="position: fixed;right:-200px;height: 100vh;z-index: 0" src="@/assets/imgs/signin.jpg"  >
  </div>
</template>

<script>
export default {
  name: "SignIn",
  data: () => ({
    valid: true,
    password: '',
    passwordRules: [
      v => !!v || 'Пароль не указан',
      v => (/\d/.test(v) && /[a-zA-Z]/.test(v)) || 'Пароль должен содержать цифры и латинские буквы',
    ],
    email: '',
    emailRules: [
      v => !!v || 'E-mail не указан',
      v => /.+@.+\..+/.test(v) || 'Некорректный e-mail ',
    ],
    select: null,

    checkbox: false,
  }),

  created(){
    if(this.$store.state.token != null && this.$store.state.user != null  ){
      if(this.$store.state.user.isTeacher){
        this.$router.replace("teacher/")
      }
      else {
        this.$router.replace("student/")
      }
    }
  },


  methods: {
    async validate () {
      if(this.$refs.form.validate()){
          await this.$store.dispatch("Login",{password:this.password, email:this.email})
      }
    },
  },
}
</script>
