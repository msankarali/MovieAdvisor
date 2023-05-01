import { observer } from 'mobx-react-lite';
import { useStore } from '../stores/store';
import { PropsWithChildren } from 'react';
import Modal from '../components/ModalComponent';
import { NavLink, Outlet, useLocation } from 'react-router-dom';
import LoginComponent from '../components/LoginComponent';

const AppLayout = ({ children }: PropsWithChildren) => {

    const { modalStore, commonStore } = useStore();

    function openLoginModal() {
        modalStore.open({ name: "login", body: <LoginComponent /> });
    }

    function logout() {
        commonStore.token = null;
    }

    return (
        <>
            <div className='container'>
                <div className='row center'>
                    <img className='logo' src="/logo.png" />
                </div>
                <div className='row'>
                    <div className='col menu'>
                        <NavLink to={'/'}>
                            <button className='btn'>HOME</button>
                        </NavLink>
                        <NavLink to={'movies'}>
                            <button className='btn'>MOVIES</button>
                        </NavLink>
                        <button className='btn'>REVIEWS</button>
                        <button className='btn'>RECOMMENDATIONS</button>
                        {/* <button className='btn'>WATCHLIST</button> */}
                        {commonStore.token === null &&
                            <button className='btn' onClick={openLoginModal}>LOGIN</button>
                        }
                        {commonStore.token !== null &&
                            <button className='btn' onClick={logout}>LOGOUT</button>}
                    </div>
                </div>
                <Outlet />
                {modalStore.modalChain.map(modal => <Modal name={modal.name} body={modal.body} />)}
            </div >
        </>
    )
}

export default observer(AppLayout);