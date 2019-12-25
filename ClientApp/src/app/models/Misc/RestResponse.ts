export class RestResponse extends Response {
    public responseKey: string;
    public responseMessage: string;

    public error: any;
    public message: string;
    public name: string;
    public ok: boolean;
    public status: number;
    public statusText: string;
    public url: string;
}
