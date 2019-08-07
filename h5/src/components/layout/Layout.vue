<template>
<div class="wy">
    <app-top></app-top>
    <app-mid></app-mid>
    <app-bot></app-bot>
</div>
</template>

<script>
//头部
import Top from './Top';
//内容
import Mid from './Mid';
//底部
import Bot from './Bot';

export default {
    data() {
        return {}
    },
    components: {
        'app-top': Top,
        'app-mid': Mid,
        'app-bot': Bot
    }
}
</script>