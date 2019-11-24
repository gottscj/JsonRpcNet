export class TypeDefinitionsService {
  pathRef = "#/";
  typeRef = "$ref";
  inheritanceProp = "allOf";

  constructor(apiInfo) {
    this.apiInfo = apiInfo;
  }

  isReferenceType(type) {
    return typeof type === "object" && this.typeRef in type;
  }

  isInheritance(type) {
    return this.inheritanceProp in type;
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

    if (this.isInheritance(typeDefinition)) {
      const inheritedProps = this.getTypeReferenceDefinition(
        typeDefinition.allOf[0]
      ).properties;
      typeDefinition = typeDefinition.allOf[1];
      typeDefinition.properties = {
        ...typeDefinition.properties,
        ...inheritedProps
      };
    }

    return typeDefinition;
  }

  createDefaultObject(type) {
    let paramTypeDef = type;

    if (this.isReferenceType(type)) {
      paramTypeDef = this.getTypeReferenceDefinition(type);
    }

    switch (paramTypeDef.type.toLowerCase()) {
      case "object": {
        let paramType = {};
        for (const property in paramTypeDef.properties) {
          paramType[property] = this.createDefaultObject(
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
              console.warn(`Unsupported string format ${paramTypeDef.format}. Format name will be used, which may not be a valid value.`);
              return paramTypeDef.format;
            }
          }
        } else if ("enum" in paramTypeDef) {
          return paramTypeDef.enum[0];
        }
        return "string";
      }
      case "array": {
        return [this.createDefaultObject(paramTypeDef.items)];
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
Date.prototype.toIsoString = function () {
  var tzo = -this.getTimezoneOffset(),
    dif = tzo >= 0 ? "+" : "-",
    pad = function (num) {
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
