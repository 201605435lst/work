import "./index.less"
import { defineComponent, reactive, ref } from "vue"
import { useRouter } from "vue-router"
import logo from "../../assets/logo.png"

export default defineComponent({
  name: "Login",
  components: {},
  setup(props, ctx) {
    const router = useRouter()

    const loginInfo = reactive({
      username: 'bdmin',
      password: '123456'
    })

    const login = (data: any) => {
      console.log("login", data)
      router.push("/")
    }
    return {
      loginInfo,
      login
    }
  },
  render() {
    return (
      <div class="login">
        <img src={logo} />
        <van-form onSubmit={this.login}>
          <van-field
            v-model={this.loginInfo.username}
            name="用户名"
            label="用户名"
            placeholder="用户名"
            rules={[{ required: true, message: "请填写用户名" }]}
          />
          <van-field
            v-model={this.loginInfo.password}
            type="password"
            name="密码"
            label="密码"
            placeholder="密码"
            rules={[{ required: true, message: "请填写密码" }]}
          />
          <div style="margin: 16px;">
            <van-button round block type="info" native-type="submit">
              提交
            </van-button>
          </div>
        </van-form>
      </div>
    )
  }
})
