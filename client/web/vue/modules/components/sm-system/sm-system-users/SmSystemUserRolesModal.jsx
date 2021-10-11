import { ModalStatus } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import ApiRole from '../../sm-api/sm-system/Role';
import ApiUser from '../../sm-api/sm-system/User';

let apiUser = new ApiUser();
let apiRole = new ApiRole();

export default {
  name: 'SmSystemUserRolesModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    organizationId: { type: String, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      userIds: [],
      roles: [],
      rolesCheckedId: [],
      rolesIsNotDefault: [],
    };
  },
  computed: {
    title() {
      return utils.getModalTitle(this.status);
    },
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    rolesChecked() {
      return this.rolesCheckedId;
    },
    indeterminate() {
      return !!this.rolesCheckedId.length && this.rolesCheckedId.length < this.roles.length;
    },
    isCheckedAll() {
      return this.rolesCheckedId.length === this.roles.length;
    },
  },
  created() {
    this.initAxios();
    this.initRoles();
  },
  methods: {
    initAxios() {
      apiUser = new ApiUser(this.axios);
      apiRole = new ApiRole(this.axios);
    },
    async initRoles() {
      let response = await apiRole.getList({
        organizationId: this.organizationId,
        public: true,
        isAssignRoles: true,
      });
      if (requestIsSuccess(response)) {
        let rolesItem = response.data.items;
        this.roles = rolesItem.filter(item => !item.isDefault);

      }
    },
    async edit(user) {
      this.initRoles();
      this.userIds = user;
      this.status = ModalStatus.Edit;
      if (user.length === 1) {
        // 获取当前用户的角色
        // console.log(this.userIds[0]);
        let response = await apiUser.getRoles(this.userIds[0], this.organizationId);
        let roleIds = [];
        if (requestIsSuccess(response)) {
          roleIds = response.data.items.map(item => item.id);
          // console.log(roleIds);
        }
        this.$nextTick(() => {
          this.rolesCheckedId = roleIds;
        });
      } 
    },

    close() {
      this.rolesCheckedId=[];
      this.status = ModalStatus.Hide;
    },

    async ok() {
      if (this.status === ModalStatus.Edit) {
        let data = {
          userIds: this.userIds,
          roleIds: this.rolesCheckedId,
          organizationId: this.organizationId,
        };
        // console.log(data);
        let response = await apiUser.updateRoles(data);
        if (requestIsSuccess(response)) {
          this.$message.success('操作成功');
          this.close();
          this.$emit('success');
        }
      } else {
        this.close();
      }
    },
  },
  render() {
    return (
      <a-modal
        title={`${this.title}角色`}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.ok}
      >
        {this.roles.length > 0 ? (
          <a-form form={this.form}>
            <a-form-item>
              <a-checkbox
                indeterminate={this.indeterminate}
                checked={this.isCheckedAll}
                onChange={event => {
                  this.rolesCheckedId = event.target.checked ? this.roles.map(item => item.id) : [];
                }}
              >
                全选
              </a-checkbox>
            </a-form-item>
            <a-form-item>
              <a-checkbox-group
                style="width:100%"
                value={this.rolesChecked}
                onChange={value => {
                  // console.log(value);
                  this.rolesCheckedId = value;
                }}
              >
                <a-row>
                  {this.roles.map(item => {
                    return (
                      <a-col span="8">
                        <a-checkbox value={item.id}>
                          {item.name}
                          {item.isPublic ? '（共享）' : ''}
                        </a-checkbox>
                      </a-col>
                    );
                  })}
                </a-row>
              </a-checkbox-group>
            </a-form-item>
          </a-form>
        ) : (
          <a-list dataSource={[]} />
        )}
      </a-modal>
    );
  },
};
