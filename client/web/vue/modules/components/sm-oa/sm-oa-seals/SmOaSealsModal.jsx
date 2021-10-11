import { ModalStatus } from '../../_utils/enum';
import { requestIsSuccess, objFilterProps, getMemberType, getFileUrl } from '../../_utils/utils';
import ApiSeal from '../../sm-api/sm-oa/Seal';
import ApiAccount from '../../sm-api/sm-system/Account';
import SmSystemMenbers from '../../sm-system/sm-system-member-select';
import SmFileManageModal from '../../sm-file/sm-file-manage-modal';

import './style/index';

let apiSeal = new ApiSeal();
let apiAccount = new ApiAccount();

// 定义表单字段常量
const formFields = [
  'name',
  'type',
  'sealRltMembers',//授权用户，若为空，则只有创建人员可用
  'isPublic',//是否公开
  'password',
  'enabled',//是否有效
  'imageId',
];

export default {
  name: 'SmOaSealsModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      seals: null,
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
      iValue: null,//所选图片信息，用来判断图片是否选择正确
      iFile: null,//所选文件
      sealRltMembers: [], //授权用户
      withoutCode: false, //是否免密
      sealIdValue: null,
      FileManageVisible: false,//文件选择框可见性
      url: null,//所选文件路径
    };
  },
  computed: {
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
  },
  watch: {},

  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiSeal = new ApiSeal(this.axios);
      apiAccount = new ApiAccount(this.axios);
    },

    async add() {
      this.status = ModalStatus.Add;
      this.initAxios();
      let response = await apiAccount.getAppConfig();
      if (requestIsSuccess(response)) {
        let _user = response.data.currentUser;
        this.$nextTick(() => {
          this.form.resetFields();
          this.form.setFieldsValue({ sealRltMembers: [{ id: _user.id, type: getMemberType(3) }] });
        });
      } else {
        this.$nextTick(() => {
          this.form.resetFields();
          this.form.setFieldsValue();
        });
      }
    },

    async update(id) {
      this.status = ModalStatus.View;
      this.sealIdValue = id;
      this.refresh();
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.sealIdValue = null;
      this.status = ModalStatus.Hide;
      this.loading = false;
      this.url = null;
      this.iFile = null;
      this.sealRltMembers = [];
    },

    //初始化计划列表
    async refresh() {
      if (this.status == ModalStatus.View) {
        let response = await apiSeal.get(this.sealIdValue);
        if (requestIsSuccess(response)) {
          let _reportRltUsers = [];
          let _seals = response.data;
          this.seals = {
            ..._seals,
            isPublic: false,
            sealRltMembers: _seals.sealRltMembers ? _seals.sealRltMembers.map(item => (
              _reportRltUsers.push(
                {
                  id: item.memberId,
                  type: getMemberType(item.memberType),
                })),
            ) : [],
            imageId: _seals.image,
          };
          this.seals.sealRltMembers = _reportRltUsers;
          this.$nextTick(() => {
            let values = objFilterProps(this.seals, formFields);
            this.form.setFieldsValue(values);
            console.log(_seals);
            this.url = getFileUrl(_seals.image.url);
            this.iValue = { id: _seals.imageId };
          });
        }
      }
    },

    // 数据提交
    ok() {
      if (this.iValue == 'error') {
        this.$message.error('请检查您的图片信息，只能上传jpg、png、gif等格式且大小不能超过2MB！');
        return false;
      }
      if (this.iValue == null) {
        console.log(this.iValue);
        this.form.setFieldsValue({ imageId: '' });
      } else {
        this.form.setFieldsValue({ imageId: 'pass' });
      }
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _values = JSON.parse(JSON.stringify(values));

          let data = {
            ..._values,
            password: _values['password'] ? _values['password'] : null,
            sealRltMembers: this.sealRltMembers ? this.sealRltMembers : [],
            imageId: this.iValue.id,
          };
          this.loading = true;
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiSeal.create(data);
            this.iValue = null;
          } else if (this.sealIdValue) {
            //加密
            response = await apiSeal.update({ id: this.sealIdValue, isPublic: true, password: data.password, type: 1 });
          }

          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
      this.loading = false;
    },


    beforeUpload(file) {
      this.iFile = file;
      if (file.length == 0) {
        this.iValue = file;
        return;
      }

      const isJpgOrPng = file[0].type === '.jpg' || file[0].type === '.png' || file[0].type === '.gif' || file[0].type === '.jpeg';
      if (!isJpgOrPng) {
        this.iValue = 'error';
        this.$message.error('只能上传jpg、png、gif等格式');
      }
      const isLt2M = file[0].size / 1024 / 1024 < 2;
      if (!isLt2M) {
        this.iValue = 'error';
        this.$message.error('图片大小必须小于2MB!');
      }
      if (isJpgOrPng && isLt2M) {
        this.iValue = file[0];
        this.url = getFileUrl(this.iValue.url);
      }
    },

    //打开文件选择框
    openSmFileManage() {
      this.isVisible = true;
    },

    //当再次改变密码时，确认密码框置空
    handlePasswordConfirmBlur(value) {
      const conform = this.form.getFieldValue('confirm');
      if (conform && value != conform) {
        this.$nextTick(() => {
          this.form.setFieldsValue({ confirm: undefined });
        });
      }
    },

    //确认密码的三个方法
    handleConfirmBlur(e) {
      const value = e.target.value;
      this.confirmDirty = this.confirmDirty || !!value;
    },
    compareToFirstPassword(rule, value, callback) {
      const form = this.form;
      if (value && value !== form.getFieldValue('password')) {
        callback('您前后两次输入的密码不一致!');
      } else {
        callback();
      }
    },
    validateToNextPassword(rule, value, callback) {
      const form = this.form;
      if (value && this.confirmDirty) {
        form.validateFields(['confirmPassword'], { force: true });
      }
      callback();
    },

    //是否免密
    isWithoutCode(e) {
      this.withoutCode = e.target.value;
    },
  },

  render() {
    return (
      <div class="oASeals">
        <a-modal
          title={this.status == ModalStatus.View ? `编辑签章` : `新增签章`}
          visible={this.visible}
          onCancel={this.close}
          confirmLoading={this.loading}
          destroyOnClose={true}
          forceRender={false}
          okText="确定"
          onOk={this.ok}
        >
          <a-form form={this.form}>
            <a-row gutter={24}>
              <a-col sm={24} md={24}>
                <a-form-item label="签章名称" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-input
                    axios={this.axios}
                    disabled={this.status == ModalStatus.View}
                    placeholder={this.status == ModalStatus.View ? '' : '请输入签章名称'}
                    v-decorator={[
                      'name',
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: '请输入签章名称',
                            whitespace: true,
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item label="签章类型" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-radio-group
                    disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'type',
                      {
                        rules: [
                          {
                            required: true,
                            message: '请授权签章类型',
                          },
                        ],
                      },
                    ]}
                  >
                    <a-radio value={0}>
                      个人签名
                    </a-radio>
                    <a-radio value={1}>
                      单位印章
                    </a-radio>
                  </a-radio-group>
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <div class="middleCol">
                  <a-form-item label="授权用户" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <SmSystemMenbers
                      axios={this.axios}
                      height={50}
                      disabled={this.status == ModalStatus.View}
                      showUserTab={true}
                      placeholder={'请选择授权用户'}
                      onChange={value => this.sealRltMembers = value}
                      v-decorator={[
                        'sealRltMembers',
                        {
                          initialValue: [],
                          rules: [
                            {
                              required: true,
                              message: '请选择授权用户',
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </div>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item label="免密签名" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-radio-group
                    onChange={event => this.isWithoutCode(event)}
                    disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'isPublic',
                      {
                        rules: [
                          {
                            required: true,
                            message: '请确认是否免密',
                          },
                        ],
                      },
                    ]}
                  >
                    <a-radio value={true}>
                      是
                    </a-radio>
                    <a-radio value={false}>
                      否
                    </a-radio>
                  </a-radio-group>
                </a-form-item>
              </a-col>

              {this.withoutCode == false || this.sealIdValue != null ?
                <div>
                  <a-col sm={24} md={24}>
                    <a-form-item label="签章密码" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                      <a-input
                        type="password"
                        placeholder={this.status == ModalStatus.View ? '' : '请输入密码'}
                        onChange={(value) => this.handlePasswordConfirmBlur(value)}
                        v-decorator={[
                          'password',
                          {
                            initialValue: null,
                            rules: [
                              {
                                required: true,
                                message: '请输入密码',
                                whitespace: true,
                              },
                              {
                                validator: this.validateToNextPassword,
                              },
                            ],
                          },
                        ]}
                      />
                    </a-form-item>
                  </a-col>

                  <a-col sm={24} md={24}>
                    <a-form-item label="确认密码" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                      <a-input
                        type="password"
                        placeholder={this.status == ModalStatus.View ? '' : '请确认密码'}
                        v-decorator={[
                          'confirm',
                          {
                            initialValue: null,
                            rules: [
                              {
                                required: true,
                                message: '请确认密码',
                                whitespace: true,
                              },
                              {
                                validator: this.compareToFirstPassword,
                              },
                            ],
                          },
                        ]}
                        onFocus={this.handleConfirmBlur}
                      />
                    </a-form-item>
                  </a-col>
                </div>
                :
                undefined
              }

              <a-col sm={24} md={24}>
                <a-form-item label="是否有效" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-radio-group
                    disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'enabled',
                      {
                        rules: [
                          {
                            required: true,
                            message: '请授权是否有效',
                          },
                        ],
                      },
                    ]}
                  >
                    <a-radio value={true}>
                      是
                    </a-radio>
                    <a-radio value={false}>
                      否
                    </a-radio>
                  </a-radio-group>
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <div class="bottomCol">
                  <a-form-item label="签名图片" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-card
                      border={true}
                      hoverable={true}
                      style="width: 38%;"
                      disabled={this.status == ModalStatus.View}
                      onClick={() => this.FileManageVisible = true}
                      v-decorator={[
                        'imageId',
                        {
                          rules: [
                            {
                              required: true,
                              message: '请选择签章图片',
                            },
                          ],
                        },
                      ]}
                    >
                      {this.url ?
                        <img
                          style="height:147px"
                          slot="cover"
                          alt="example"
                          src={this.url}
                        />
                        :
                        <div
                          style="height:100%;text-align: center;"
                        >
                          <si-upload style="font-size:40px;margin:13px" />
                          <div>
                            上传图片，请<a>点击上传</a>
                          </div>
                          <span style={"color:#C0C0C0;font-size: 1px"}>注：仅支持jpg、png、gif等格式</span>
                        </div>
                      }
                    </a-card>
                  </a-form-item>
                </div>
              </a-col>
            </a-row>
          </a-form>
        </a-modal>
        {/* 文件选择模态框 */}
        <SmFileManageModal
          axios={this.axios}
          visible={this.FileManageVisible}
          selected={this.iFile}
          multiple={false}
          onOk={value => {
            value = [value];
            this.beforeUpload(value);
          }}
          onChange={v => { this.FileManageVisible = v; }}
        />


      </div>
    );
  },
};