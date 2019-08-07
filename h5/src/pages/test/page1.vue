<template>
<div id="testpage1">
    testpage1
</div>
</template>

<script>
export default {
    data() {
        return {
            editForm: {

            },
            rules: {
                username: [{
                    required: true,
                    message: '请输入账号',
                    trigger: 'blur'
                }],
                password: [{
                        required: true,
                        message: '请输入密码',
                        trigger: 'blur'
                    },
                    // { min: 8, max: 16, message: '长度在 8 到 16 个字符', trigger: 'blur' }
                ],
                verifycode: [{
                    required: true,
                    message: '请输入验证码',
                    trigger: 'blur'
                }]
            }
        }
    },
    props: {

    },
    mounted: function () {

    },
    computed: {

    },
    methods: {

    }
}
</script>

<style>
</style>
