<template>
  <div>
    <v-card class="d-none d-md-block " style="border-radius: 0px"   elevation="0"  >
      <v-navigation-drawer
          width="270px"
          fixed
          permanent
          style="z-index: 1000"
      >
        <template v-slot:prepend>
          <v-list-item two-line>
            <v-list-item-avatar color="orange">{{$store.state.user.name.toUpperCase()[0]}}</v-list-item-avatar>

            <v-list-item-content>
              <v-list-item-title>{{$store.state.user.name}}</v-list-item-title>
              <v-list-item-subtitle>{{$store.state.user.email}}</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </template>

        <v-divider></v-divider>

        <v-list dense>
          <v-list-item-group
              v-model="group"
              active-class="primary--text text--accent-4"
          >
            <v-list-item v-if="!$store.state.user.isTeacher" @click="goTo(i.path)" v-for="i in studentItems" :key="i.id">
              <v-list-item-icon >
                <v-icon>{{i.icon}}</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>{{i.title}}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>

            <v-list-item v-if="$store.state.user.isTeacher" @click="goTo(i.path)" v-for="i in teacherItems" :key="i.id">
              <v-list-item-icon >
                <v-icon>{{i.icon}}</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>{{i.title}}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>

            <v-divider></v-divider>

            <v-list-item  @click="changeTheme()">
              <v-list-item-icon >
                <v-icon>mdi-brightness-4</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>Сменить тему</v-list-item-title>
              </v-list-item-content>
            </v-list-item>

          </v-list-item-group>
        </v-list>
        <template v-slot:append>
          <div class="pa-2">
            <v-btn @click="exit()" block>
              Выйти
            </v-btn>
          </div>
        </template>
      </v-navigation-drawer>

      <div v-if="$store.state.windowWidth>960" class="p-3" style="margin-left:270px ">
        <router-view class="d-none d-md-block "></router-view>
      </div>
    </v-card>

  <v-card
      style="border-radius: 0px"
      elevation="0"
      class="d-block d-md-none  mx-auto overflow-hidden"
  >
    <v-app-bar
        color="primary"
        dark
        fixed
        prominent
        height="55px"
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>

      <v-spacer></v-spacer>

<!--      <v-btn icon>-->
<!--        <v-icon>mdi-magnify</v-icon>-->
<!--      </v-btn>-->
    </v-app-bar>

    <v-navigation-drawer
        width="270"
        height="100vh"
        style="z-index: 1000"
        v-model="drawer"
        fixed
        temporary
    >

      <template v-slot:prepend>
        <v-list-item two-line>
          <v-list-item-avatar color="orange">{{$store.state.user.name.toUpperCase()[0]}}</v-list-item-avatar>

          <v-list-item-content>
            <v-list-item-title>{{$store.state.user.name}}</v-list-item-title>
            <v-list-item-subtitle>{{$store.state.user.email}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </template>

      <v-divider></v-divider>

      <v-list  dense>
        <v-list-item-group
            v-model="group"
            active-class="primary--text text--accent-4"
        >
          <v-list-item v-if="!$store.state.user.isTeacher" @click="goTo(i.path)" v-for="i in studentItems" :key="i.id">
            <v-list-item-icon >
              <v-icon>{{i.icon}}</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{i.title}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item v-if="$store.state.user.isTeacher" @click="goTo(i.path)" v-for="i in teacherItems" :key="i.id">
            <v-list-item-icon >
              <v-icon>{{i.icon}}</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{i.title}}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider></v-divider>

          <v-list-item  @click="changeTheme()">
            <v-list-item-icon >
              <v-icon>mdi-brightness-4</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>Сменить тему</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>

      <template v-slot:append>
        <div class="pa-2">
          <v-btn @click="exit()" block>
            Выйти
          </v-btn>
        </div>
      </template>
    </v-navigation-drawer>

    <div v-if="$store.state.windowWidth<960" class="p-3 mt-12">
      <router-view></router-view>
    </div>

  </v-card>
</div>
</template>

<script>


export default {
  name: "MainTeacher",
  data () {
    return {
      drawer: false,
      group: null,
      teacherItems:[
        {id:1, title:"Классы", icon:"mdi-account-group-outline", path:"TeacherClasses"},
        {id:3, title:"Задания для лабораторных", icon:"mdi-clipboard-list-outline", path:"TeacherTasks"}
      ],
      studentItems:[
        {id:2, title:"Классы", icon:"mdi-account-group-outline", path:"StudentClasses"},
        {id:4, title:"Задания", icon:"mdi-clipboard-list-outline", path:"StudentLabSolutions"},
      ]
    }
  },

  watch: {
    group () {
      this.drawer = false
    },
  },

  methods:{
    changeTheme(){
      this.$store.commit("setIsDark",!this.$store.state.isDark);
      this.$vuetify.theme.dark = this.$store.state.isDark
    },
    goTo(path){
      if(this.$route.name != path){
        this.$router.push({name:path})
      }
    },
    exit(){
      this.$store.dispatch("Exit")
      this.$router.replace('/')
    }

  }
}
</script>

