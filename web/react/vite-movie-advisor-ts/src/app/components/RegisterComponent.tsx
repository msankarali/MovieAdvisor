import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";

const RegisterComponent = () => {

    const { userStore, modalStore } = useStore();

    return (
        <div className="center">
            <div style={{ textAlign: 'center', fontWeight: 100, fontSize: '28px' }}>BE ONE OF US</div><br />
            <input type="text" placeholder="First name" onChange={x => userStore.firstName = x.target.value}></input><br /><br />
            <input type="text" placeholder="Last name" onChange={x => userStore.lastName = x.target.value}></input><br /><br />
            <input value={userStore.email} type="text" placeholder="Email" onChange={x => userStore.email = x.target.value}></input><br /><br />
            <input value={userStore.password} type="password" placeholder="Password" onChange={x => userStore.password = x.target.value}></input><br /><br />
            <button className="btn" style={{ width: '100%', fontWeight: 200 }} onClick={async () => {
                await userStore.register({ email: userStore.email, firstName: userStore.firstName, lastName: userStore.lastName, password: userStore.password })
            }}>Register</button>
        </div >
    );
}

export default observer(RegisterComponent);