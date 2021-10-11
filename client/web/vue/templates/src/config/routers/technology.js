// eslint-disable-next-line
import { PageView,FlowLayout } from '@/layouts'
import permissionTechnology from 'snweb-module/es/_permissions/sm-technology';
import permissionResource from 'snweb-module/es/_permissions/sm-resource';
import permissionFile from 'snweb-module/es/_permissions/sm-file';

export default {
  path: 'technology',
  name: 'technology',
  component: PageView,
  meta: { title: '技术管理', keepAlive: false, icon: 'snippets', permission: [permissionTechnology.GroupName] },
  children: [
    {
      path: 'file-manage-resource',
      name: 'file-manage-resource',
      component: () => import('@/views/file/FileManage'),
      meta: { title: '资料管理', keepAlive: false, permission: [permissionFile.FileManager.Default] },
    },
    {
      path: 'file-manage-paper',
      name: 'file-manage-paper',
      component: () => import('@/views/file/FileManagePaper'),
      meta: { title: '图纸管理', keepAlive: false, permission: [permissionFile.FileManager.Default] },
    },
    {
      path: 'file-manage-join',
      name: 'file-manage-join',
      component: () => import('@/views/file/FileManageJoin'),
      meta: { title: '三维会审', keepAlive: false, permission: [permissionFile.FileManager.Default] },
    },
    {
      path: 'disclose',
      name: 'disclose',
      component: () => import('@/views/technology/Disclose'),
      meta: {
        title: '可视化交底',
        keepAlive: false,
        permission: [permissionTechnology.Discloses.Default],
        showInMenu: true,
      },
    },
    {
      path: 'quantity',
      name: 'quantity',
      component: FlowLayout,
      meta: { title: '工程量统计', keepAlive: false, permission: [permissionTechnology.GroupName] },
      children:[
        {
          path: 'quantity-list',
          name: 'quantity-list',
          component: () => import('@/views/technology/Quantity'),
          meta: {
            title: '工程量清单',
            keepAlive: false,
            permission: [permissionTechnology.Quanitys.Default],
            showInMenu: true,
          },
        },
        {
          path: 'material-plan',
          name: 'material-plan',
          component: () => import('@/views/technology/MaterialPlan'),
          meta: {
            title: '用料计划',
            keepAlive: false,
            permission: [permissionTechnology.MaterialPlans.Default],
            showInMenu: true,
          },
        },
        {
          path: 'material-plan-approval',
          name: 'material-plan-approval',
          component: () => import('@/views/technology/MaterialPlanApproval'),
          meta: {
            title: '用料计划审批',
            keepAlive: false,
            permission: [permissionTechnology.MaterialPlans.Default],
            showInMenu: true,
          },
        },
      ],
    },
    {
      path: 'component-qrcode-generate',
      name: 'component-qrcode-generate',
      component: () => import('@/views/componenttrack/ComponentQrCodeGenerate'),
      meta: {
        title: '二维码构件跟踪',
        keepAlive: false,
        permission: [permissionTechnology.ComponentRltQRCodes.Default],
        showInMenu: true,
      },
    },
    {
      path: 'file-manage-checkout',
      name: 'file-manage-checkout',
      component: () => import('@/views/file/FileApproveManage'),
      meta: { title: '检验批管理', keepAlive: false, permission: [permissionFile.FileManager.Default] },
    },
  ],
};
