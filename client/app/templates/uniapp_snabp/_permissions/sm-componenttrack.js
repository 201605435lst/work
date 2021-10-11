import Common from './common';
const { Export,Dot} = Common;
const GroupName = 'AbpComponentTrack';

const GroupNameComponentTracks = GroupName + Dot + 'ComponentRltQRCodes';
const ComponentRltQRCodes = {
  Default: ComponentRltQRCodes,
  Export: ComponentRltQRCodes + Dot + Export,
};
export default {   
  GroupName,
  ComponentRltQRCodes,
};
