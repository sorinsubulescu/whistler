import { StartupService } from './startup.service';

export const startupServiceFactory = (startupService: StartupService): Function => {
    return (): Promise<any> => startupService.load();
}