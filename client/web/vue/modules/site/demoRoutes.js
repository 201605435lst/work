import { demo as components } from './modules/components';
import { demo as basic } from './modules/basic';
import { demo as bpm } from './modules/bpm';
import { demo as cms } from './modules/cms';
import { demo as common } from './modules/common';
import { demo as crPlan } from './modules/cr-plan';
import { demo as viewD3 } from './modules/d3';
import { demo as emerg } from './modules/emerg';
import { demo as exam } from './modules/exam';
import { demo as file } from './modules/file';
import { demo as resource } from './modules/resource';
import { demo as statistics } from './modules/statistics';
import { demo as stdBasic } from './modules/std-basic';
import { demo as system } from './modules/system';
import { demo as oa } from './modules/oa';
import { demo as project } from './modules/project';
import { demo as problem } from './modules/problem';
import { demo as dashboard } from './modules/dashboard';
import { demo as report } from './modules/report';
import { demo as regulation } from './modules/regulation';
import { demo as task } from './modules/task';
import { demo as schedule } from './modules/schedule';
import { demo as material } from './modules/material';
import { demo as technology } from './modules/technology';
import { demo as costmanagement } from './modules/costmanagement';
import { demo as safe } from './modules/safe';
import { demo as quality } from './modules/quality';
import { demo as constructionBase } from './modules/constructionBase';
import { demo as construction } from './modules/construction';

import { demo as componenttrack } from './modules/componenttrack';
export default [
  {
    path: 'sm-namespace-module',
    component: () => import('@/components/sm-namespace/sm-namespace-module/demo/index.vue'),
  },
  {
    path: 'sm-namespace-module-cn',


    component: () => import('@/components/sm-namespace/sm-namespace-module/demo/index.vue'),
  },
  ...components,
  ...basic,
  ...bpm,
  ...safe,
  ...cms,
  ...common,
  ...crPlan,
  ...exam,
  ...file,
  ...resource,
  ...statistics,
  ...system,
  ...stdBasic,
  ...emerg,
  ...viewD3,
  ...oa,
  ...report,
  ...project,
  ...problem,
  ...dashboard,
  ...regulation,
  ...task,
  ...schedule,
  ...material,
  ...technology,
  ...costmanagement,
  ...quality,
  //进度管理
  ...constructionBase,
  //施工计划管理
  ...construction,
  ...componenttrack,



];
