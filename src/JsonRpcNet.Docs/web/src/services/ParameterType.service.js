export class ParameterTypeService {
  pathRef = "#/";
  typeRef = "$ref";

  static provider;

  static initialize(apiInfo) {
    if (ParameterTypeService.provider) {
      throw new Error(
        "Trying to initialize ParameterTypeService more than once"
      );
    }

    ParameterTypeService.provider = new ParameterTypeService(apiInfo);
  }

  static getProvider() {
    if (!ParameterTypeService.provider) {
      throw Error("DefinitionsProvider is not initialized");
    }

    return ParameterTypeService.provider;
  }

  constructor(apiInfo) {
    this.apiInfo = apiInfo;
  }

  isReferenceType(type) {
    return typeof type === "object" && this.typeRef in type;
  }

  getTypeReferenceDefinition(type, rootDefinition = null) {
    var reference = type[this.typeRef];
    if (!reference) {
      throw Error(`
        The provided type ${type} does not contain a ${this.typeRef} property.
      `);
    }

    if (!reference.startsWith(this.pathRef)) {
      throw Error(`
        Invalid reference name. Must start with ${this.pathRef}
      `);
    }

    const definition = rootDefinition ? rootDefinition : this.apiInfo;

    const referencePath = reference.replace(this.pathRef, "");
    const pathParts = referencePath.split("/");
    let typeDefinition = null;
    pathParts.forEach(p => {
      typeDefinition = typeDefinition ? typeDefinition[p] : definition[p];
      if (!typeDefinition) {
        throw new Error(
          `Could not find type definition for reference path ${reference}`
        );
      }
    });

    return typeDefinition;
  }

  unrollParameterType(type) {
    let paramTypeDef = type;

    if (ParameterTypeService.getProvider().isReferenceType(type)) {
      paramTypeDef = ParameterTypeService.getProvider().getTypeReferenceDefinition(
        type
      );
    }

    switch (paramTypeDef.type.toLowerCase()) {
      case "object": {
        let paramType = {};
        for (const property in paramTypeDef.properties) {
          paramType[property] = this.unrollParameterType(
            paramTypeDef.properties[property]
          );
        }
        return paramType;
      }
      case "string": {
        if ("format" in paramTypeDef) {
          switch (paramTypeDef.format.toLowerCase()) {
            case "date-time":
              return new Date().toIsoString();
            default: {
              // TODO: support more formats
              // eslint-disable-next-line
              console.error(`Unsupported string format ${paramTypeDef.format}`);
              break;
            }
          }
        } else if ("enum" in paramTypeDef) {
          return paramTypeDef.enum.join(" | ");
        }
        return "string";
      }
      case "boolean": {
        return false;
      }
      case "integer": {
        return 0;
      }
      case "number": {
        return 1.23;
      }
      default: {
        return "<error>";
      }
    }
  }
}

/* eslint-disable */
Date.prototype.toIsoString = function() {
  var tzo = -this.getTimezoneOffset(),
    dif = tzo >= 0 ? "+" : "-",
    pad = function(num) {
        var norm = Math.floor(Math.abs(num));
        return (norm < 10 ? "0" : "") + norm;
    };
  return this.getFullYear() +
    "-" + pad(this.getMonth() + 1) +
    "-" + pad(this.getDate()) +
    "T" + pad(this.getHours()) +
    ":" + pad(this.getMinutes()) +
    ":" + pad(this.getSeconds()) +
    dif + pad(tzo / 60) +
    ":" +
    pad(tzo % 60);
};
