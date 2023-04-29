import { Modal } from "../stores/modalStore";
import { useStore } from "../stores/store";

const ModalComponent = (modal: Modal) => {
    const { modalStore } = useStore();

    return (
        <div className="modal">
            <div className="modal-content">
                <span className="close" onClick={() => modalStore.close(modal.name)}>
                    X
                </span>
                {modal.body}
            </div>
        </div>
    )
}

export default ModalComponent;