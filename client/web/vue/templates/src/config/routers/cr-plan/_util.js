import { PageState, RepairTagKeys } from 'snweb-module/es/_utils/enum';

export function getGroupWithRepairTag(routes, repairTagKey, routPrefix) {
  let _group = routes.map(item => Object.assign({}, item));
  if (repairTagKey && routPrefix) {
    _group.forEach(item => {
      let handle = item.props;
      item.path = routPrefix + '.' + item.path;
      item.name = routPrefix + '.' + item.name;
      if (handle) {
        item.props = route => {
          return handle(route, repairTagKey);
        };
      }
    });
  }
  return _group;
}

export const repairTagKeyConfigs = [
  {
    tag: RepairTagKeys.RailwayWired,
    routePrefix: 'railway-wired',
    routeMeta: {
      title: '有线科',
      keepAlive: false,
      icon: 'calendar',
      permission: [],
    },
  },
  {
    tag: RepairTagKeys.RailwayHighSpeed,
    routePrefix: 'railway-high-speed',
    routeMeta: {
      title: '高铁科',
      keepAlive: false,
      icon: 'calendar',
      permission: [],
    },
  },
];

export const routePrefixes = repairTagKeyConfigs.map(item => item.routePrefix);
