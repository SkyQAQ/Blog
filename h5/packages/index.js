import Vue from 'vue';

import WyHeader from '../packages/header/src/main';
import WyContent from '../packages/content/src/main';
import WyGrid from '../packages/grid/src/main';
import WyImport from '../packages/import/src/main';
import WyInputTree from '../packages/input-tree/src/main';
import WySelect from '../packages/select/src/main';

var components = [
    WyHeader,
    WyContent,
    WyGrid,
    WyImport,
    WyInputTree,
    WySelect,
];

export default {
    install: function (Vue, opts) {
        components.map(function (component) {
            Vue.component(component.name, component);
        });
    }
}