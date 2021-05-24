import Vue from 'vue'
import VueRouter from 'vue-router'

import StartPage from "../views/StartPage";
import Main from "../views/Main";
// Auth
import RegisterStudent from "../views/Auth/RegisterStudent";
import RegisterTeacher from "../views/Auth/RegisterTeacher";
import SignIn from "../views/Auth/SignIn";
// Student
import StudentClasses from "../views/Student/StudentClasses";
import StudentClassInfo from "../views/Student/StudentClassInfo";
import StudentLabSolutions from "../views/Student/StudentLabSolutions";
import StudentLabSolutionInfo from "../views/Student/StudentLabSolutionInfo";
// Teacher
import TeacherClassInfo from "../views/Teacher/TeacherClassInfo"
import TeacherClasses from "../views/Teacher/TeacherClasses"
import TeacherLabInfo from "../views/Teacher/TeacherLabInfo";
import TeacherTasks from "../views/Teacher/TeacherTasks";

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'StartPage',
    component: StartPage
  },
  {
    path: '/signin',
    name: 'SignIn',
    component: SignIn
  },
  {
    path: '/register-student',
    name: 'RegisterStudent',
    component: RegisterStudent
  },
  {
    path: '/register-teacher',
    name: 'RegisterStudent',
    component: RegisterTeacher
  },
  {
    path: '/teacher/',
    name: 'MainTeacher',
    component: Main,
    children:[
      {
        path: 'task',
        name: 'TeacherTasks',
        component: TeacherTasks
      },
      {
        path: 'class',
        name: 'TeacherClasses',
        component: TeacherClasses
      },
      {
        path: 'class/:classId',
        name: 'TeacherClassInfo',
        component: TeacherClassInfo
      },
      {
        path: 'class/:classId',
        name: 'TeacherClassInfo',
        component: TeacherClassInfo
      },
      {
        path: 'class/:classId/lab/:labId',
        name: 'TeacherLabInfo',
        component: TeacherLabInfo
      },
    ]
  },
  {
    path: '/student',
    name: 'MainStudent',
    component: Main,
    children:[
      {
        path: 'lab-solutions',
        name: 'StudentLabSolutions',
        component: StudentLabSolutions
      },
      {
        path: 'class',
        name: 'StudentClasses',
        component: StudentClasses
      },
      {
        path: 'class/:classId',
        name: 'StudentClassInfo',
        component: StudentClassInfo
      },
      {
        path: 'class/:classId/lab/:labId/student/:studentId',
        name: 'StudentLabSolutionInfo',
        component: StudentLabSolutionInfo
      },
    ]
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
