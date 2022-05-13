import {
    ICanvasShapeTemplateGroup,
    ICanvasShapeTemplate,
    ICanvasShape,
    ICanvasElementType,
    IAPiTemplate,
    ApiResult, ISearchRepo
} from '../models';
import { v4 as uuidv4 } from 'uuid';
import KeycloakService from './KeycloakService';

export class TemplateService {
    static saveTemplateTimeoutHandle = null;

    public createNewRootShape() {
        let rootShape: ICanvasShape = {
            id: uuidv4(),
            name: "Root",
            type: ICanvasElementType.Shape,
            canHaveChildren: true,
            elements: [],
            position: null,
            properties: [],
            connectionPoints: [],
            width: 0,
            height: 0
        };

        return rootShape;
    }

    public getTemplateGroups(templates: Array<IAPiTemplate>, filterText?: string) {
        filterText = (filterText || '').toLowerCase();
        templates = templates.filter(template => !filterText || template.name.toLowerCase().indexOf(filterText) > -1);

        let categories = templates
            .map(x => x.category)
            .filter((val, index, self) => self.indexOf(val) === index); // get unique values

        let templateGroups: Array<ICanvasShapeTemplateGroup> = categories.map(x => {
            return { name: x, items: templates.filter(template => template.category === x) }
        });

        return templateGroups;
    }

    public saveTemplate(template: IAPiTemplate) {
        TemplateService.saveTemplateTimeoutHandle = setTimeout(() => {
            TemplateService.saveTemplateTimeoutHandle = null;
            fetch(`/api/templates`, {
                method: "POST",
                body: JSON.stringify(template),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
        }, 500);
    }

    public saveRepo(repo: IAPiTemplate, username : string) {
        TemplateService.saveTemplateTimeoutHandle = setTimeout(() => {
            TemplateService.saveTemplateTimeoutHandle = null;
            fetch(`/api/repo/` + username, {
                method: "POST",
                body: JSON.stringify(repo),
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${KeycloakService.getToken()}`,
                }
            })
        }, 500);
    }

    public deleteTemplate(templateId: string) {
        TemplateService.saveTemplateTimeoutHandle = setTimeout(() => {
            TemplateService.saveTemplateTimeoutHandle = null;
            fetch(`/api/templates` + "/" + templateId, {
                method: "DELETE",
            })
        }, 500);
    }


    public static searchPublicRepos<T>(query: string): Promise<ISearchRepo[]> {
        return fetch(`/api/repo/s?query=` + query, {
            method: 'GET',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${KeycloakService.getToken()}`,
            }
        })
            .then(response => response.json() as Promise<ApiResult<Array<ISearchRepo>>>)
            .then(apiResult => {
                return apiResult.data;
            });
    }


    public static getPublicRepo<T>(repo: ISearchRepo): Promise<IAPiTemplate> {
        return fetch(`/api/repo?user=` + repo.user + `&workflowName=` + repo.workflowName, {
            method: 'GET',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${KeycloakService.getToken()}`,
            }
        })
            .then(response => response.json() as Promise<ApiResult<IAPiTemplate>>)
            .then(apiResult => {
                return apiResult.data;
            });
    }
}
