import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import permissionFile from 'snweb-module/es/_permissions/sm-file';


export default {
  path: 'file',
  name: 'file',
  component: PageView,
  meta: { title: '文件管理', keepAlive: true, icon: 'file', permission: [permissionFile.GroupName] },
  children: [
    {
      path: 'file-manage',
      name: 'file-manage',
      component: () => import('@/views/file/FileManage'),
      meta: { title: '文件管理', keepAlive: false, permission: [permissionFile.FileManager.Default] },
    },
    {
      path: 'oss-config',
      name: 'oss-config',
      component: () => import('@/views/file/OssConfig'),
      meta: { title: 'OSS服务配置', keepAlive: false, permission: [permissionFile.OssConfig.Default] },
    },
  ],
};