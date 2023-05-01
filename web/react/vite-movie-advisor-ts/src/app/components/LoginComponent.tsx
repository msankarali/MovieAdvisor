import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";
import RegisterComponent from "./RegisterComponent";

const LoginComponent = () => {

    const { userStore, modalStore } = useStore();

    return (
        <div className="center">
            <div style={{ textAlign: 'center', fontWeight: 100, fontSize: '28px' }}>WELCOME</div><br />
            <input value={userStore.email} type="text" placeholder="Email" onChange={x => userStore.email = x.target.value}></input><br /><br />
            <input value={userStore.password} type="password" placeholder="Password" onChange={x => userStore.password = x.target.value}></input><br /><br />
            <button className="btn" style={{ width: '100%', fontWeight: 200 }} onClick={async () => userStore.login({ email: userStore.email, password: userStore.password })}>Login</button><br /><br />
            <button className="btn" style={{ width: '100%', fontWeight: 200 }} onClick={x => {
                modalStore.open({ name: "registerPage", body: <RegisterComponent /> });
            }}>Register</button>
        </div >
    );
}

export default observer(LoginComponent);