import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { useStore } from './app/stores/store'
import { observer } from 'mobx-react-lite'
import { Modal } from './app/stores/modalStore'
import AppLayout from './app/layout/AppLayout'
import LoginComponent from './app/components/LoginComponent'

const App = () => {

  const { userStore, modalStore } = useStore();

  const [loginRequest, setLoginRequest] = useState({ email: "", password: "" });

  return (
    <AppLayout>
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <p>
          Email: <input type='text' onChange={x => setLoginRequest({ ...loginRequest, email: x.target.value })}></input>
        </p>
        <p>
          Email: <input type='text' onChange={x => setLoginRequest({ ...loginRequest, password: x.target.value })}></input>
        </p>
        <p>
          <button onClick={() => {
            userStore.login(loginRequest);
            modalStore.open({
              name: "login", body: <LoginComponent />
            });
          }}>Send! {userStore.someData}</button>
        </p>
      </div >
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </AppLayout>
  )
}

export default observer(App);