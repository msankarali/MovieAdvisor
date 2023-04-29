import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";

const LoginComponent = () => {

    const { userStore, modalStore } = useStore();

    return (
        <>
            crazy sample here. I have this data here: {userStore.someData}
            <button onClick={() => modalStore.open({
                name: "somethinn",
                body: <>
                    <p>
                        Another modal here!!!
                    </p>
                    <p>
                        <button onClick={() => { userStore.someData = "YES CRAZYYYY!" }}>CLICK HERE!</button>
                    </p>
                    <p>and manipulate the data...</p>
                </>
            })}>open another modal</button>
        </>
    );
}

export default observer(LoginComponent);