import Vue from 'vue';
import pagesRouter from '@/pages/router'
import store from '@/store'
import VueRouter from 'vue-router';
Vue.use(VueRouter);

const Login = resolve => require(['@/components/account/login.vue'], resolve);
const Layout = resolve => require(['@/components/layout/Layout.vue'], resolve);
const Home = resolve => require(['@/components/home/Home.vue'], resolve);
const Error_403 = resolve => require(['@/components/error/403.vue'], resolve);
const IFrame = resolve => require(['@/components/layout/IFrame.vue'], resolve);

const router = new VueRouter({
    routes: [{
            path: '/login',
            name: 'login',
            component: Login,
            meta: {
                allowAnyone: true
            }
        },
        {
            path: '/403',
            name: 'Error_403',
            component: Error_403,
            meta: {
                allowAnyone: true
            }
        },
        {
            path: '/',
            redirect: {
                name: 'home'
            },
            meta: {
                allowAnyone: true,
                keepAlive: true,
            },
            component: Layout,
            children: [{
                    path: 'home',
                    name: 'home',
                    component: Home,
                    meta: {
                        allowAnyone: true
                    }
                },
                {
                    path: '/iframe',
                    name: 'iframe',
                    component: IFrame,
                    meta: {
                        allowAnyone: true
                    }
                }
            ]
        },
        ...generatePagesRouter(),
    ]
});

// 加载页面路由
function generatePagesRouter() {
    return pagesRouter;
}

// 全局路由钩子
router.beforeEach((to, from, next) => {
    if (to.meta && to.meta.allowAnyone) {
        next();
        return;
    }
    // 检查是否登录
    checkLoginstatus(to, next);
    // 检测是否有路由权限
    checkAccessGrant(to, next);
});

function checkLoginstatus(to, next) {
    const user = store.state.user;
    if (!user || !user.UserId) {
        if (to.name === 'home') {
            next({
                name: 'login'
            });
            return;
        } else {
            next({
                name: 'login',
                query: {
                    redirect: to.fullPath
                }
            });
            return;
        }
    }
    next();
    return;
}

function checkAccessGrant(to, next) {
    var isOk = false;
    var routes = JSON.parse(localStorage.getItem(_const.Key_AccessRoute)) || [];
    if (routes.length > 0) {
        for (var i in routes) {
            if (routes[i] === to.path) {
                isOk = true;
                break;
            }
        }
    }
    if (isOk) {
        next();
        return;
    } else {
        next({
            name: 'Error_403',
        });
        return;
    }
}

Vue.router = router;
export default router;