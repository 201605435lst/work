const SmComponentQrCodeGenerate = {
  category: 'Modules',
  type: 'Componenttrack',
  title: 'SmComponentQrCodeGenerate',
  subtitle: '构件二维码生成',
  demos: [
    {
      path: 'sm-component-qr-code-generate',
      component: () => import('@/components/sm-componenttrack/sm-component-qr-code-generate/demo/index.vue'),
        
    },
    {
      path: 'sm-component-qr-code-generate-cn',
      component: () => import('@/components/sm-componenttrack/sm-component-qr-code-generate/demo/index.vue'),
    },
  ],
};
const SmComponentQrCode = {
  category: 'Modules',
  type: 'Componenttrack',
  title: 'SmComponentQrCode',
  subtitle: '构件二维码管理',
  demos: [
    {
      path: 'sm-component-qr-code',
      component: () => import('@/components/sm-componenttrack/sm-component-qr-code/demo/index.vue'),
        
    },
    {
      path: 'sm-component-qr-code-cn',
      component: () => import('@/components/sm-componenttrack/sm-component-qr-code/demo/index.vue'),
    },
  ],
};
// const SmComponentTrack = {
//   category: 'Modules',
//   type: 'Componenttrack',
//   title: 'SmComponentTrack',
//   subtitle: '构件跟踪预设',
//   demos: [
//     {
//       path: 'sm-component-track',
//       component: () =>
//         import('@/components/sm-componenttrack/sm-component-track/demo/index.vue'),
//     },
//     {
//       path: 'sm-component-track-cn',
//       component: () =>
//         import('@/components/sm-componenttrack/sm-component-track/demo/index.vue'),
//     },
//   ],
// };
// const SmComponentTrackProcess = {
//   category: 'Modules',
//   type: 'Componenttrack',
//   title: 'SmComponentTrackProcess',
//   subtitle: '跟踪构件流程',
//   demos: [
//     {
//       path: 'sm-component-track-process',
//       component: () =>
//         import('@/components/sm-componenttrack/sm-component-track-process/demo/index.vue'),
//     },
//     {
//       path: 'sm-component-track-process-cn',
//       component: () =>
//         import('@/components/sm-componenttrack/sm-component-track-process/demo/index.vue'),
//     },
//   ],
// };
export const modules = {
  SmComponentQrCode,
  SmComponentQrCodeGenerate,
    
};
export const demo = [
  ...SmComponentQrCode.demos,
  ...SmComponentQrCodeGenerate.demos,
];
    