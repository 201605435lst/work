import "./style/index";
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import * as utils from '../../_utils/utils';
import ApiArchivesCategory from "../../sm-api/sm-project/ArchivesCategory";
import moment from 'moment';
import ProjectCardTree from '../sm-project-dossier/src/ProjectCardTree';
import ProjectArchivesManage from './src/ProjectArchivesManage';
import SmProjectArchivesCatrgotyModal from './SmProjectArchivesCatrgotyModal';
import ProjectEmptyManage from "../sm-project-dossier/src/ProjectEmptyManage";
let apiArchivesCategory = new ApiArchivesCategory();

export default {
  name: 'SmProjectArchives',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      record: null,
      loading: false,
      parentId: null,
      queryParams: {
        name: null,
      },
      archives: [],
    };
  },
  computed: {

  },
  watch: {

  },
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiArchivesCategory = new ApiArchivesCategory(this.axios);
    },
    refresh() {

    },
    add() {

    },
  },
  render() {
    return (
      <div class="sm-project-archives">
        <ProjectCardTree
          axios={this.axios}
          permissions={this.permissions}
          api={apiArchivesCategory}
          onView={(item) => {
            this.record = item;
            this.parentId = item ? item.id : null;
          }}
          type="ProjectArchivesCatrgoty"
        />
        {this.record  ? 
          <ProjectArchivesManage
            parentId={this.parentId}
            axios={this.axios}
            permissions={this.permissions}
          /> 
          :
          <ProjectEmptyManage 
            message={this.record && !this.record.parentId?"请选择你要管理的档案":'请选择你要管理的档案'}
          />
        } 
      </div>
    );
  },
};
