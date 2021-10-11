import { ref, defineComponent, onMounted } from "vue"
import { useRouter } from "vue-router"
import './index.less'
export default defineComponent({
  name: "File",
  setup() {
    const router = useRouter()
    const logOut = () => {
      router.push("/login")
    }
    const viewFile = () => {
      router.push("/file")
    }

    const win = window as any
    const bridge = win.uni ? win.uni : win

    console.log(bridge)

    // 注册 UniApp 桥接准备事件
    onMounted(() => {
      document.addEventListener("UniAppJSBridgeReady", () => {
        console.log("WebView 桥接已经准备好了，可以调用 UniApp 的接口了")
        bridge.postMessage({
          data: {
            action: "首次发布测试消息"
          }
        })
        // bridge.parent.postMessage("data_shiming", '*')
        bridge.getEnv((res: any) => {
          console.log("当前环境：" + JSON.stringify(res))
        })
      })
    })

    const scan = () => {
      console.log('onBtnScan')
      bridge.postMessage({
        data: {
          action: "scan"
        }
      })
    }

    return { logOut, viewFile, scan }
  },
  render() {
    return (
      <div class="user" >
        <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br />
        用户中心
        <br />
        <van-button onClick={this.scan}>扫码</van-button>
        <br />
        <van-button onClick={this.viewFile} type="primary">
          查看文件
        </van-button>
        <br /> <br />
        <van-button onClick={this.logOut} type="primary">
          退出
        </van-button>
      </div>
    )
  }
})
