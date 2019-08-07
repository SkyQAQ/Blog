import axios from 'axios'
import store from '../store';

export default {
    install(Vue,  options) {
        // Add a request interceptor
        axios.interceptors.request.use(function (config) {
            const auth = store.state.auth;
            // Do something before request is sent
            if(auth != null && auth.access_token != null && auth.access_token.length > 0){
                // 添加请求token
                config.headers.Authorization = auth.token_type + ' ' + auth.access_token;
            }
            return config;
        }, function (error) {
            // Do something with request error
            return Promise.reject(error);
        });
        Vue.prototype.$http = Vue.http = axios;
    }
}