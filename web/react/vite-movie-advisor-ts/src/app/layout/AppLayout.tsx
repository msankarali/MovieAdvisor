import { observer } from 'mobx-react-lite';
import { useStore } from '../stores/store';
import { PropsWithChildren } from 'react';
import Modal from '../components/ModalComponent';

const AppLayout = ({ children }: PropsWithChildren) => {

    const { modalStore } = useStore();

    return (
        <>
            <h1>Blaa</h1>
            {children}
            {modalStore.modalChain.map(modal => <Modal name={modal.name} body={modal.body} />)}
        </>
    )
}

export default observer(AppLayout);