<template>
<div>

  <div class="mainContainer">
    <div class="mb-5">
      <span class="mainTitle primary--text" ><b>Платформа поддержки <br> вирутальных лабораторных работ по физике</b></span>
    </div>

    <div class="mb-3">
      <v-btn
          color="primary"
          elevation="2"
          @click="$router.push('signin')"
      >Войти</v-btn>
    </div>

    <div class="mb-3">
      <v-btn
          color="primary"
          elevation="2"
          @click="$router.push('register-teacher')"
      >Зарегестрироваться как учитель</v-btn>
    </div>
    <div class="mb-3">
      <v-btn
          class="btn-small-sm"
          color="primary"
          elevation="2"
          @click="$router.push('register-student')"
      >Зарегестрироваться как ученик</v-btn>
    </div>
  </div>

  <div class="mainImg">
    <img src="../assets/imgs/main.png"  >
  </div>

</div>
</template>

<script>

export default {
  name: "Main"
}
</script>

