import Vue from 'vue';
import store from '../store';
import router from '../router';

export default {
    install(Vue, options) {
        var that = this;
        // Add a response interceptor
        Vue.http.interceptors.response.use(function (response) {
            // Do something with response data
            return response;
        }, function (error) {
            // Do something with response error
            if (error.response.status === 401 && (error.response.data.mssg === 'Expired token' || error.response.data.mssg === 'Invalid token' )) {
                return that.refreshToken(error.config);
            }
            return Promise.reject(error.response);
        });
        Vue.prototype.$auth = Vue.auth = this;
        // Vue.prototype.$store = Vue.store = store;
    },
    refreshToken: function(config){
        const data = 'grant_type=refresh' + '&refresh_token=' + store.state.auth.refresh_token + '&mac=00000000';
        return Vue.http.post('api/auth/token', data).then((res)=>{
            this.storeToken(res.data.data);
            return Vue.http.request(config);
        }).catch((error) =>{
            return Promise.reject(error);
        }); 
    },
    storeToken: function(token){
        store.commit('update_auth', token);
    },
    storeUser: function(user){
        store.commit('update_user', user);
    },
    clearToken: function(){
        store.commit('reset_auth_user');
        localStorage.removeItem(_const.Key_AccessRoute);
        localStorage.removeItem(_const.Key_UploadLimit);
        router.push({ name: 'login' });
    }
}