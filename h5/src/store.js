import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

const USER_AUTH_LOCORAGE_KEY = 'userauth';
// 当 store 初始化后调用
const USER_AUTH_PLUGIN = store => {
    if (localStorage.getItem(USER_AUTH_LOCORAGE_KEY)) {
        let user_auth = JSON.parse(localStorage.getItem(USER_AUTH_LOCORAGE_KEY));
        store.state.user = user_auth.user;
        store.state.auth = user_auth.auth;
    }
    store.subscribe((mutation, state) => {
        // 每次 mutation 之后调用
        // mutation 的格式为 { type, payload }
        if (mutation.type == 'update_user' || mutation.type == 'update_auth' || mutation.type == 'update_user_avatar' || mutation.type == 'update_user_name') {
            let user_auth = {
                user: state.user,
                auth: state.auth
            }
            localStorage.setItem(USER_AUTH_LOCORAGE_KEY, JSON.stringify(user_auth));
        } else if (mutation.type == 'reset_auth_user') {
            localStorage.removeItem(USER_AUTH_LOCORAGE_KEY);
        }
    })
}

// 可以用Module分模块，详情地址 https://vuex.vuejs.org/zh/guide/modules.html
export default new Vuex.Store({
    // state为单一状态树，在state中需要定义我们所需要管理的数组、对象、字符串等等，
    // 只有在这里定义了，在vue.js的组件中才能获取你定义的这个对象的状态。
    state: {
        auth: {
            access_token: null,
            refresh_token: null,
            token_type: null,
            expires_in: null,
        },
        user: {
            UserId: null,
            UserAccount: null,
            UserName: null,
            UserEmail: null,
            UserAvatar: null,
            UserRoles: [],
        },
        table_height: 0,
        top_height: 280,
    },
    // 更改store中state状态的唯一方法就是提交mutation，就很类似事件。
    // 每个mutation都有一个字符串类型的事件类型和一个回调函数，我们需要改变state的值就要在回调函数中改变。
    // 我们要执行这个回调函数，那么我们需要执行一个相应的调用方法：store.commit。
    mutations: {
        update_auth: function(state, data) {
            state.auth = data;
        },
        update_user: function(state, data) {
            state.user = data;
        },
        update_user_avatar: function(state, data) {
            state.user.UserAvatar = data;
        },
        update_user_name: function(state, data) {
            state.user.UserName = data;
        },
        update_table_height: function(state, data) {
            state.table_height = data;
        },
        reset_auth_user: function(state) {
            state.auth = {
                access_token: null,
                refresh_token: null,
                token_type: null,
                expires_in: null,
            };
            state.user = {
                UserId: null,
                UserAccount: null,
                UserName: null,
                UserEmail: null,
                UserAvatar: null,
                UserRoles: [],
            };
        },
    },
    // action可以提交mutation，在action中可以执行store.commit，而不是直接变更状态，而且action中可以有任何的异步操作。
    actions: {

    },
    // 我们在 store 中定义“getter”（可以认为是 store 的计算属性）。
    // 就像计算属性一样，getter 的返回值会根据它的依赖被缓存起来，且只有当它的依赖值发生了改变才会被重新计算。
    getters: {

    },
    plugins: [USER_AUTH_PLUGIN]
});