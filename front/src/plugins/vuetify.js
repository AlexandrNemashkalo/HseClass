import Vue from 'vue'
import Vuetify from 'vuetify/lib'

Vue.use(Vuetify)

export default new Vuetify({
    theme: {
        dark: false,
        themes: {
            light: {
                primary: '#652CD4'
            },
            dark: {
                primary: '#652CD4'
            }
        },
    },
})
