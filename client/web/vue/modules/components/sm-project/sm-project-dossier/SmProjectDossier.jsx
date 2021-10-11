import './style/index';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import * as utils from '../../_utils/utils';
import ApiDossierCategory from '../../sm-api/sm-project/DossierCategory';
import moment from 'moment';
import ProjectCardTree from './src/ProjectCardTree';
import ProjectEmptyManage from "../sm-project-dossier/src/ProjectEmptyManage";
import ProjectDossierManage from './src/ProjectDossierManage';
import SmProjectDossierCatrgotyModal from './SmProjectDossierCatrgotyModal';
import SmProjectArchivesCatrgotyTreeSelect from '../sm-project-archives-catrgoty-tree-select/SmProjectArchivesCatrgotyTreeSelect';
let apiDossierCategory = new ApiDossierCategory();

export default {
  name: 'SmProjectDossier',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      record: null,
      loading: false,
      parentId: null,
      archivesCatrgotyId:null,
      archives: [],
    };
  },
  computed: {
    
  },
  watch: {},
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiDossierCategory = new ApiDossierCategory(this.axios);
    },
    refresh() {},
    add() {},
  },
  render() {
    return (
      <div class="sm-project-dossier">
        <ProjectCardTree 
          axios={this.axios} 
          api={apiDossierCategory} 
          permissions={this.permissions}
          archivesCatrgotyId={this.archivesCatrgotyId} 
          type="ProjectDossierCatrgoty"
          onView={(item) => {
            this.record = item;
            console.log(this.record);
            this.parentId = item ? item.id : null;
          }}
        >
          <SmProjectArchivesCatrgotyTreeSelect
            class="project-dossier-button"
            axios={this.axios}
            slot="cardTreeSlot"
            onChange={(value)=>{
              this.archivesCatrgotyId=value;
              this.record=null;
            }}
          />
        </ProjectCardTree>
        {this.record && this.record.isFile ?
          <ProjectDossierManage 
            permissions={this.permissions}
            parentId={this.parentId}
            axios={this.axios} />:
          <ProjectEmptyManage 
            message={this.record && this.record.isFile?"请选择":'请选择你要管理的卷宗'}
          />
        }

        {/* 添加/编辑模板 */}
        <SmProjectDossierCatrgotyModal
          ref="SmProjectDossierCatrgotyModal"
          axios={this.axios}
          onSuccess={(action, data) => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
