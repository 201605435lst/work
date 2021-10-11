import { modules as components } from './modules/components';
import { modules as basic } from './modules/basic';
import { modules as bpm } from './modules/bpm';
import { modules as cms } from './modules/cms';
import { modules as common } from './modules/common';
import { modules as crPlan } from './modules/cr-plan';
import { modules as d3 } from './modules/d3';
import { modules as emerg } from './modules/emerg';
import { modules as exam } from './modules/exam';
import { modules as file } from './modules/file';
import { modules as resource } from './modules/resource';
import { modules as statistics } from './modules/statistics';
import { modules as stdBasic } from './modules/std-basic';
import { modules as system } from './modules/system';
import { modules as oa } from './modules/oa';
import { modules as project } from './modules/project';
import { modules as problem } from './modules/problem';
import { modules as dashboard } from './modules/dashboard';
import { modules as alarm } from './modules/alarm';
import { modules as report } from './modules/report';
import { modules as regulation } from './modules/regulation';
import { modules as task } from './modules/task';
import { modules as schedule } from './modules/schedule';
import { modules as material } from './modules/material';
import { modules as technology } from './modules/technology';
import { modules as costmanagement } from './modules/costmanagement';
import { modules as safe } from './modules/safe';
import { modules as quality } from './modules/quality';
import { modules as constructionBase } from './modules/constructionBase';
import { modules as construction } from './modules/construction';



import { modules as componenttrack } from './modules/componenttrack';
export default {
  template: {
    category: 'Components',
    subtitle: '模板',
    type: 'Template',
    title: 'SmNamespaceModule',
  },
  ...components,
  ...basic,
  ...bpm,
  ...cms,
  ...common,
  ...crPlan,
  ...d3,
  ...emerg,
  ...exam,
  ...file,
  ...resource,
  ...statistics,
  ...stdBasic,
  ...system,
  ...oa,
  ...report,
  ...project,
  ...problem,
  ...dashboard,
  ...alarm,
  ...regulation,
  ...task,
  ...schedule,
  ...material,
  ...technology,
  ...costmanagement,
  ...safe,
  ...quality,
  // 进度管理
  ...constructionBase,
  // 施工计划管理
  ...construction,
  ...componenttrack,



};
