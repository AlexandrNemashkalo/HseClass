<template>
  <div style="height: 100vh;" >

    <v-card
        class="regContainer"
        elevation="2"
    >

      <v-card
          elevation="0"
          class="formCard"
      >
        <v-card-title>Регистрация учителя</v-card-title>

        <v-form
            ref="form"
            v-model="valid"
            lazy-validation
        >

          <v-text-field
              v-model="name"
              :rules="nameRules"
              label="ФИО"
              required
          ></v-text-field>


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
          >Зарегистрироваться</v-btn>

          <span style="font-size: 12px">Есть аккаунт?
              <a @click="$router.push('signin')" class="primary--text">Войти</a>
          </span>
        </v-form>


      </v-card>
    </v-card>


    <img style="position: fixed;right:0px;height: 100vh;z-index: 0" src="@/assets/imgs/reg.jpg"  >
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
    name: '',
    nameRules: [
      v => !!v || 'ФИО не указано'
    ],
    email: '',
    emailRules: [
      v => !!v || 'E-mail не указан',
      v => /.+@.+\..+/.test(v) || 'Некорректный e-mail ',
    ],
    select: null,

    checkbox: false,
  }),

  methods: {
    async validate () {
      if(this.$refs.form.validate()){
        var requestData ={
          email: this.email,
          name: this.name,
          password: this.password
        };
        await this.$store.dispatch("RegisterTeacher",requestData)
      }

    },
  },
}
</script>
