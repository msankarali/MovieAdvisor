import { makeAutoObservable } from 'mobx';

export interface Modal {
    name: string;
    body: JSX.Element;
}

export default class ModalStore {
    modalChain: Modal[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    open(modal: Modal) {
        this.modalChain?.push(modal);
    }

    close(modalName: string) {
        // const foundModal = this.modalChain?.find(mc => mc.name === modalName);

        // if (!foundModal) throw new Error("Modal not found!");

        this.modalChain = this.modalChain?.filter(mc => mc.name !== modalName);
    }
}