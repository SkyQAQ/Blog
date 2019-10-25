const Layout = resolve => require(['@/components/layout/Layout.vue'], resolve);

const testpage1 = resolve => require(['@/pages/test/page1.vue'], resolve);
const testpage2 = resolve => require(['@/pages/test/page2.vue'], resolve);

const roleinfolist = resolve => require(['@/pages/systemmanage/roleinfo/list.vue'], resolve);
const userinfolist = resolve => require(['@/pages/systemmanage/userinfo/list.vue'], resolve);
const menuinfomanage = resolve => require(['@/pages/systemmanage/menuinfo/manage.vue'], resolve);
const quartzlist = resolve => require(['@/pages/systemmanage/quartz/list.vue'], resolve);
const attachmentlist = resolve => require(['@/pages/systemmanage/attachment/list.vue'], resolve);
const dreaminfolist = resolve => require(['@/pages/dreaminfo/list.vue'], resolve);
const vedioinfolist = resolve => require(['@/pages/vedioinfo/list.vue'], resolve);
export default [{
        path: '/test',
        component: Layout,
        children: [{
                path: 'page1',
                name: 'page1',
                component: testpage1,
                meta: {}
            },
            {
                path: 'page2',
                name: 'page2',
                component: testpage2,
                meta: {}
            },
        ]
    },
    {
        path: '/roleinfo',
        component: Layout,
        children: [{
            path: 'list',
            name: 'roleinfolist',
            component: roleinfolist,
            meta: {}
        }, ]
    },
    {
        path: '/userinfo',
        component: Layout,
        children: [{
            path: 'list',
            name: 'userinfolist',
            component: userinfolist,
            meta: {}
        }, ]
    },
    {
        path: '/menuinfo',
        component: Layout,
        children: [{
            path: 'manage',
            name: 'menuinfomanage',
            component: menuinfomanage,
            meta: {}
        }, ]
    },
    {
        path: '/quartz',
        component: Layout,
        children: [{
            path: 'list',
            name: 'quartzlist',
            component: quartzlist,
            meta: {}
        }, ]
    },
    {
        path: '/attachment',
        component: Layout,
        children: [{
            path: 'list',
            name: 'attachmentlist',
            component: attachmentlist,
            meta: {}
        }, ]
    },
    {
        path: '/dreaminfo',
        component: Layout,
        children: [{
            path: 'list',
            name: 'dreaminfolist',
            component: dreaminfolist,
            meta: {}
        }, ]
    },
    {
        path: '/vedioinfo',
        component: Layout,
        children: [{
            path: 'list',
            name: 'vedioinfolist',
            component: vedioinfolist,
            meta: {}
        }, ]
    }
]