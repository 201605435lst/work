const SmSafeProblem = {
  category: 'Modules',
  type: 'Safe',
  title: 'SmSafeProblem',
  subtitle: '安全问题管理',
  demos: [
    {
      path: 'sm-safe-problem',
      component: () =>
        import('@/components/sm-safe/sm-safe-problem/demo/index.vue'),
    },
    {
      path: 'sm-safe-problem-cn',
      component: () =>
        import('@/components/sm-safe/sm-safe-problem/demo/index.vue'),
    },
  ],
};
const SmSafeProblemLibrary = {
  category: 'Modules',
  type: 'Safe',
  title: 'SmSafeProblemLibrary',
  subtitle: '安全问题库管理',
  demos: [
    {
      path: 'sm-safe-problem-library',
      component: () =>
        import('@/components/sm-safe/sm-safe-problem-library/demo/index.vue'),
    },
    {
      path: 'sm-safe-problem-library-cn',
      component: () =>
        import('@/components/sm-safe/sm-safe-problem-library/demo/index.vue'),
    },
  ],
};
const SmSafeRltQualityModalSelect = {
  category: 'Modules',
  type: 'Safe',
  title: 'SmSafeRltQualityModalSelect',
  subtitle: '安全和质量问题选择框',
  demos: [
    {
      path: 'sm-safe-rlt-quality-modal-select',
      component: () =>
        import('@/components/sm-safe/sm-safe-rlt-quality-modal-select/demo/index.vue'),
    },
    {
      path: 'sm-safe-rlt-quality-modal-select-cn',
      component: () =>
        import('@/components/sm-safe/sm-safe-rlt-quality-modal-select/demo/index.vue'),
    },
  ],
};
const SmSafeSpeechVideo = {
  category: 'Modules',
  type: 'Safe',
  title: 'SmSafeSpeechVideo',
  subtitle: '班前讲话',
  demos: [
    {
      path: 'sm-safe-speech-video',
      component: () =>
        import('@/components/sm-safe/sm-safe-speech-video/demo/index.vue'),
    },
    {
      path: 'sm-safe-speech-video-cn',
      component: () =>
        import('@/components/sm-safe/sm-safe-speech-video/demo/index.vue'),
    },
  ],
};
export const modules = {
  SmSafeProblem,
  SmSafeProblemLibrary,
  SmSafeSpeechVideo,
  SmSafeRltQualityModalSelect,
  
};
export const demo = [
  ...SmSafeRltQualityModalSelect.demos,
  ...SmSafeProblem.demos,
  ...SmSafeProblemLibrary.demos,
  ...SmSafeSpeechVideo.demos,
];
  