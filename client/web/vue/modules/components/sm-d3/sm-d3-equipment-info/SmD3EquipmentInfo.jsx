import SmD3EquipmentInfoFiles from '../sm-d3-equipment-info-files';
import SmD3EquipmentInfoProperties from '../sm-d3-equipment-info-properties';
import SmResourceStoreEquipmentRecord from '../../sm-resource/sm-resource-store-equipment-record';
import { getFileUrl, requestIsSuccess } from '../../_utils/utils';
import { EquipmentType } from '../../_utils/enum';
import SmFileImageView from '../../sm-file/sm-file-manage/src/component/SmFileImageView';
import ApiEquipments from '../../sm-api/sm-resource/Equipments';
import ApiComponentQrCode from '../../sm-api/sm-resource/ComponentQrCode';
import SmD3CableLocation from '../sm-d3-cable-location';
import qrcode from 'qrcode';

let apiEquipments = new ApiEquipments();
let apiComponentQrCode = new ApiComponentQrCode();

import './style/index.less';

export default {
  name: 'SmD3EquipmentInfo',
  // model: {
  //   prop: 'equipment',
  //   event: 'change',
  // },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    equipment: { type: Object, default: null }, //设备
    position: {
      type: Object,
      default: () => {
        return {
          right: '20px',
          top: '20px',
          bottom: '20px',
        };
      },
    },
  },
  data() {
    return {
      iValue: null,
      iVisible: false,
      targetFile: null,
      qrCodeVisible: false,
      qrCodeValue: null,
    };
  },

  computed: {},

  watch: {
    equipment: {
      handler: async function (value, oldValue) { },
    },
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        if (!value && this.$refs.SmD3CableLocation) {
          this.$refs.SmD3CableLocation.clearRender();
        }
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiEquipments = new ApiEquipments(this.axios);
      apiComponentQrCode = new ApiComponentQrCode(this.axios);
    },
    // 查看文件，只支持pdf 和图片
    view(file) {
      let imgtypes = ['.jpg', '.png', '.tif', 'gif'];
      if (file && imgtypes.includes(file.type)) {
        this.$refs.SmFileImageView.view(file);
      } else {
        this.$message.warning('当前文件不支持预览');
      }
    },
    async onQrCode() {
      this.qrCodeVisible = true;
      // 获取该设备构建跟踪二维码

      let respone = await apiComponentQrCode.getByInstallationEquipmentId(this.equipment.id);

      if (requestIsSuccess(respone)) {
        this.qrCodeValue = respone.data;
      } else {
        this.qrCodeValue = null;
      }

      if (this.qrCodeValue) {

        this.$nextTick(() => {
          let canvas = document.getElementById('canvas-qrcode');
          qrcode.toCanvas(canvas, JSON.stringify({ key: "equipment", value: this.qrCodeValue }), { width: 300, margin: 1 }, (error) => {
            if (error)
              this.$message.error(error);
          });
        });
      }
    },
  },

  render() {
    return (
      <sc-panel
        class="sm-d3-equipment-info"
        title="设备属性"
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        width="260px"
        animate="rightEnter"
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <template slot="icon">
          <a-icon type="unordered-list" />
        </template>

        <div class="content">
          {this.equipment ? (
            [
              <div class="info" style="flex:1;">
                <a-tabs default-active-key="1" size="small"

                >
                  <span
                    slot="tabBarExtraContent"
                    class="qrcode-button"
                    title="二维码"
                    onClick={this.onQrCode}
                  >
                    <a-icon type="qrcode" />
                  </span>

                  <a-tab-pane key="1" tab="设备属性">
                    <SmD3EquipmentInfoProperties
                      axios={this.axios}
                      value={this.equipment}
                      onCableCoreDetial={(id) => { this.$emit("cableCoreDetial", id); }}
                    />
                  </a-tab-pane>
                  <a-tab-pane key="2" tab="构件跟踪" force-render>
                    <SmResourceStoreEquipmentRecord
                      axios={this.axios}
                      value={this.equipment ? this.equipment.id : null}
                      size="small"
                    />
                  </a-tab-pane>

                  {this.equipment && this.equipment.type === EquipmentType.Cable
                    ? <a-tab-pane key="3" tab="电缆埋深" force-render>
                      <div class="cable-location-box">
                        <SmD3CableLocation
                          ref="SmD3CableLocation"
                          axios={this.axios}
                          value={this.equipment ? this.equipment.id : null}
                          onAdd={data => {
                            this.$emit('cableLocationAdd', data);
                          }}
                          size="small"
                        />
                      </div>
                    </a-tab-pane>
                    : undefined}
                </a-tabs>
              </div>,

              <div class="file" style="flex:1;">
                <a-tabs default-active-key="1" size="small">
                  <span
                    slot="tabBarExtraContent"
                    class="add-button"
                    onClick={() => {
                      this.$refs.SmD3EquipmentInfoFiles.add();
                    }}
                  >
                    <a-icon type="plus" />
                  </span>
                  <a-tab-pane key="1" tab="关联资料">
                    <SmD3EquipmentInfoFiles
                      ref="SmD3EquipmentInfoFiles"
                      axios={this.axios}
                      value={this.equipment ? this.equipment.id : null}
                      onChange={file => {
                        if (
                          file &&
                          ['.jpg', '.jpeg', '.png', '.bmp', '.gif', '.psd', '.dxf'].indexOf(
                            file.type,
                          ) > -1
                        ) {
                          this.targetFile = (
                            <div class="thumb">
                              <div class="thumb-box">
                                <img class="img" src={getFileUrl(file.url)} />
                                <a-icon
                                  class="icon"
                                  style="margin:5px 0; position: absolute; right: 30px;"
                                  type="fullscreen"
                                  onClick={() => {
                                    this.view(file);
                                  }}
                                />
                                <a-icon
                                  class="icon"
                                  style="margin:5px; position: absolute; right: 0; "
                                  type="close"
                                  onClick={() => {
                                    this.targetFile = undefined;
                                    this.$refs.SmD3EquipmentInfoFiles.targetFileIndex = null;
                                  }}
                                />
                              </div>
                            </div>
                          );
                        } else {
                          this.targetFile = null;
                        }
                      }}
                    />
                  </a-tab-pane>
                </a-tabs>
              </div>,

              this.targetFile,
            ]
          ) : (
            <span
              style="
                margin-top: 20px;
                color: #bdbdbd;
              "
            >
              无数据
            </span>
          )}
        </div>

        {/* 文件缩略图模态框*/}
        <SmFileImageView ref="SmFileImageView" />


        <a-modal
          title="构建二维码"
          visible={this.qrCodeVisible}
          onOk={() => (this.qrCodeVisible = false)}
          onCancel={() => this.qrCodeVisible = false}
        >
          <div style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            width: "100%",
            height: "300px",
            flexDirection: 'column',
          }}  >
            {!this.qrCodeValue
              ? <span> 该设备还未安装</span>
              : <span> 请用手机 App 扫码</span>}
            <canvas id="canvas-qrcode" style={{
              display: this.qrCodeValue ? "block" : "none",
            }}
            ></canvas>
          </div>
        </a-modal>
      </sc-panel>
    );
  },
};
