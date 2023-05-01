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

  const [loginRequest, setLoginRequest] = useState({ email: "", password: "" });

  return (
    <AppLayout>
      
    </AppLayout>
  )
}

export default observer(App);