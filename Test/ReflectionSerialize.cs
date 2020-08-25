using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using NUnit.Framework;

namespace Altseed2.Test
{
    /// <summary>
    /// ���t���N�V������p���ăV���A���C�Y�e�X�g���s���N���X
    /// </summary>
    [TestFixture]
    public partial class ReflectionSerialize
    {
        /// <summary>
        /// ���O�f���p�̃��C�^�[
        /// </summary>
        private static StreamWriter writer;

        private static void Serialize(string path, object item)
        {
            var direc = Path.GetDirectoryName(path);
            if (!Directory.Exists(direc)) Directory.CreateDirectory(direc);
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, item);
        }

        private static object Deserialize(string path)
        {
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(path, FileMode.Open);
            return formatter.Deserialize(stream);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Serialization()
        {
            // Serialization\�t�H���_�̐���
            if (!Directory.Exists("Serialization")) Directory.CreateDirectory("Serialization");
            // ���O�̏�����
            writer = new StreamWriter("Serialization/SerializationLog.txt", false)
            {
                AutoFlush = true
            };

            var tc = new TestCore();
            tc.Init();

            // �V���A���C�Y�p�̃T���v���I�u�W�F�N�g�����Ɏ擾
            foreach (var (_, info) in ReflectionSources.Info)
            {
                // �V���A���C�Y���Ȃ��悤�Ƀ}�[�N����Ă����ꍇ�X�L�b�v
                if (!info.DoSerialization) continue;
                // �o�C�i���t�@�C����
                var replacedTypeName = info.Type.FullName.Replace('.', '_').Replace('`', '_');

                var path = $"Serialization/Binaries/{replacedTypeName}.bin";

                // �V���A���C�Y���s
                Serialize(path, info.Value);
                // �V���A�����C�Y�Ő��������t�@�C�������邩�`�F�b�N
                if (!System.IO.File.Exists(path)) throw new AssertionException($"Failed to Serialze object\nType: {info.Type.FullName}");

                // �f�V���A���C�Y���s
                var deSerialized = Deserialize(path);
                // �f�V���A���C�Y���ʂ�null����Ȃ����`�F�b�N
                Assert.NotNull(deSerialized);

                // ���O�f���o��
                writer.WriteLine($"-{info.Type}-");

                // �����o�[��
                EnumerateMembers(info.Value, deSerialized, info.Type, info.FieldInfos, info.PropertyInfos, info.MethodsInfos, $"{info.Type.FullName}");

                // ���s
                writer.WriteLine();
            }

            tc.End();
        }

        /// <summary>
        /// �w�肵�������o�[��񋓂��Ĕ�r����
        /// </summary>
        /// <param name="obj1">��r����I�u�W�F�N�g1</param>
        /// <param name="obj2">��r����I�u�W�F�N�g2</param>
        /// <param name="type"><paramref name="obj1"/>��<paramref name="obj2"/>�ɂ�����<see cref="Type"/></param>
        /// <param name="fields"><paramref name="obj1"/>��<paramref name="obj2"/>���r����ۂɗ񋓂���t�B�[���h</param>
        /// <param name="properties"><paramref name="obj1"/>��<paramref name="obj2"/>���r����ۂɗ񋓂���v���p�e�B</param>
        /// <param name="methods"><paramref name="obj1"/>��<paramref name="obj2"/>���r����ۂɗ񋓂��郁�\�b�h�Ƃ��̈���</param>
        /// <param name="name">���O�f���o�����ɗp����v�f�̖��O</param>
        private static void EnumerateMembers(object obj1, object obj2, Type type, FieldInfo[] fields, PropertyInfo[] properties, (MethodInfo, object[])[] methods, string name)
        {
            // Base�N���X�̌���
            if (!ReflectionSources.Info.ContainsKey(type) && type.IsClass && type.BaseType != null && type.BaseType != typeof(object)) EnumerateMembers(obj1, obj2, type.BaseType, fields, properties, methods, name);

            // OptionalValueProvider�ɔ�r����v�f���L�q����Ă���ꍇ���̗v�f�����o���Ĕ�r
            if (ReflectionSources.Info.TryGetValue(type, out var refInfo))
            {
                var values = refInfo.OptionalValueProvider?.Invoke(obj1, obj2);
                if (values != null)
                    foreach (var (valueName, valueType, comparison1, comparison2) in values)
                        Compare(comparison1, comparison2, valueType, $"{name}.{valueName}");
            }

            // �t�B�[���h���r
            foreach (var field in fields) Compare(field.GetValue(obj1), field.GetValue(obj2), field.FieldType, $"{name}.{field.Name}");
            // �v���p�e�B���r
            foreach (var property in properties) Compare(property.GetValue(obj1), property.GetValue(obj2), property.PropertyType, $"{name}.{property.Name}");
            // ���\�b�h�̕Ԃ�l���r
            foreach (var (method, args) in methods) Compare(method.Invoke(obj1, args), method.Invoke(obj2, args), method.ReturnType, $"{name}.{method.Name}");
        }

        /// <summary>
        /// �w�肵���v�f�̔�r���s��
        /// </summary>
        /// <param name="obj1">��r����v�f1</param>
        /// <param name="obj2">��r����v�f2</param>
        /// <param name="type"><paramref name="obj1"/>��<paramref name="obj2"/>�ɂ�����<see cref="Type"/></param>
        /// <param name="name">���O�f���o�����ɗp����v�f�̖��O</param>
        private static void Compare(object obj1, object obj2, Type type, string name)
        {
            if (obj1 == null)
            {
                // obj1��null�̎�->obj2��null�ł���ׂ�
                if (obj2 != null) throw new AssertionException($"{name}\nobj1: null\nobj2: Not null");
                writer.WriteLine($"{name}<{type.FullName}> : {obj1.ToLogText()} - {obj2.ToLogText()}");
                return;
            }
            // obj1��null�łȂ���->obj2��null�łȂ��ׂ�
            if (obj2 == null) throw new AssertionException($"{name}\nobj1: Not null\nobj2: null");

            // ��r�Ώۂ̗v�f�̔�r���@��ReflectionSources�ɋL�ڂ���Ă���ꍇ�͂��̓��e�����Ƃɔ�r���s��
            if (ReflectionSources.Info.TryGetValue(type, out var refInfo))
            {
                EnumerateMembers(obj1, obj2, type, refInfo.FieldInfos, refInfo.PropertyInfos, refInfo.MethodsInfos, name);
                return;
            }
            //
            // [����]
            // ValueType�ɑ΂���object.Equal(object, object)��object == object�����s����ƕK��false�ɂȂ�
            //
            switch (type)
            {
                //
                // float�Cdouble�Cdecimal�Ȃǂ̏����_�n�̏ꍇ�C�덷���������Ĕ�r
                //
                case Type t when t == typeof(float):
                    if (System.Math.Abs((float)obj1 - (float)obj2) >= MathHelper.MatrixError) throw new AssertionException($"{name}\nNot Equals\nobj1: {obj1}\nobj2: {obj2}");
                    break;
                case Type t when t == typeof(double):
                    if (System.Math.Abs((double)obj1 - (double)obj2) >= MathHelper.MatrixError) throw new AssertionException($"{name}\nNot Equals\nobj1: {obj1}\nobj2: {obj2}");
                    break;
                case Type t when t == typeof(decimal):
                    if (System.Math.Abs((decimal)obj1 - (decimal)obj2) >= (decimal)MathHelper.MatrixError) throw new AssertionException($"{name}\nNot Equals\nobj1: {obj1}\nobj2: {obj2}");
                    break;
                // Enum�̏ꍇ�͐��l�ɒ����Ĕ�r
                case Type t when t.IsEnum:
                    if ((int)obj1 != (int)obj2) throw new AssertionException($"{name}\nNot Equals\nobj1: {obj1}\nobj2: {obj2}");
                    break;
                // TStruct?�̏ꍇ�͓�����TStruct�̒l���r
                // �� ����̑����null�`�F�b�N������Ă��邽�߁C����switch���ł�obj1��obj2�͋��ɔ�null
                case Type t when t.Name == typeof(Nullable<>).Name:
                    Compare(type.GetProperty("Value").GetValue(obj1), type.GetProperty("Value").GetValue(obj2), type.GetGenericArguments()[0], name);
                    return;
                // KeyValuePair�̏ꍇ��Key��Value�ł��ꂼ���r
                // �� System.Collections.Generic.Dictionary`2 �΍�
                case Type t when t.Name == typeof(KeyValuePair<,>).Name:
                    Compare(type.GetProperty("Key").GetValue(obj1), type.GetProperty("Key").GetValue(obj2), type.GetGenericArguments()[0], $"{name}.Key");
                    Compare(type.GetProperty("Value").GetValue(obj1), type.GetProperty("Value").GetValue(obj2), type.GetGenericArguments()[1], $"{name}.Value");
                    return;
                // System.IEquatable`1 ���������Ă���ꍇ�� System.IEquatable`1.Equals��p���Ĕ�r(object.Equals(object, object)�͐�q�̗��R�̂��ߗp���Ȃ�)
                case Type t when (t.HasInterface(typeof(IEquatable<>)) && t.GetInterface(typeof(IEquatable<>).FullName).GetGenericArguments()[0] == t):
                    var IEquatableT = t.GetInterface(typeof(IEquatable<>).FullName);
                    if (!(bool)IEquatableT.GetMethod("Equals").Invoke(obj1, new[] { obj2 })) throw new AssertionException($"{name}\nNot Equals\nobj1: {obj1}\nobj2: {obj2}");
                    break;
                // foreach�\�ȏꍇobject[]�ɂ��ėv�f����r���e�v�f���r
                // ���̑���ɂ����鐧��͉��L��ToObjectArray�Q��
                case Type t when t.HasInterface(typeof(IEnumerable)):
                    var array1 = ((IEnumerable)obj1).ToObjectArray();
                    var array2 = ((IEnumerable)obj2).ToObjectArray();
                    if (array1.Length != array2.Length) throw new AssertionException($"{name}\nCollection Count is not same\nCount1: {array1.Length}\nCount2: {array2.Length}");
                    for (int i = 0; i < array1.Length; i++) Compare(array1[i], array2[i], array1[i]?.GetType(), $"{name}[{i}]");
                    return;
                // �ǂ̑���ɂ��Y�����Ȃ��ꍇ�̓t�B�[���h�ƃv���p�e�B��S�񋓂��Ĕ�r
                // �\������ςȒl(IntPtr��)���r���Ď��ʂ̂łȂ�ׂ�ReflectionSource���ŋL�q���邱��
                default:
                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    var methods = Array.Empty<(MethodInfo, object[])>();
                    var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    EnumerateMembers(obj1, obj2, type, fields, properties, methods, name);
                    break;
            }
            // ���O�f���o��
            writer.WriteLine($"{name}<{type.FullName}> : {obj1.ToLogText()} - {obj2.ToLogText()}");
        }

        /// <summary>
        /// ���t���N�V������p���Ĕ�r������e�����N���X
        /// </summary>
        public sealed class ReflectionInfo
        {
            /// <summary>
            /// <see cref="Serialization"/>����<see cref="Value"/>�̃V���A���C�Y���s�����ǂ������擾�܂��͐ݒ肷��
            /// </summary>
            public bool DoSerialization { get; set; } = true;

            /// <summary>
            /// ��r����t�B�[���h���擾�܂��͐ݒ肷��
            /// </summary>
            public FieldInfo[] FieldInfos { get => _fieldInfos; set => _fieldInfos = value ?? Array.Empty<FieldInfo>(); }
            // �Ԃ�l��null�ɂȂ��foreach���Ŏ��ʂ̂�Array.Empty<T>()��K���Ăяo������
            private FieldInfo[] _fieldInfos = Array.Empty<FieldInfo>();

            /// <summary>
            /// ��r���郁�\�b�h�Ƃ��̈������擾�܂��͐ݒ肷��
            /// </summary>
            public (MethodInfo, object[])[] MethodsInfos { get => _methodInfos; set => _methodInfos = value ?? Array.Empty<(MethodInfo, object[])>(); }
            // �Ԃ�l��null�ɂȂ��foreach���Ŏ��ʂ̂�Array.Empty<T>()��K���Ăяo������
            private (MethodInfo, object[])[] _methodInfos = Array.Empty<(MethodInfo, object[])>();

            /// <summary>
            /// �ǉ��Ŕ�r����v�f������ꍇ�͂��̒��ŕԂ�l�Ƃ��Ď�������
            /// </summary>
            public OptionalValueProvider OptionalValueProvider { get; set; }

            /// <summary>
            /// ��r����v���p�e�B���擾�܂��͐ݒ肷��
            /// </summary>
            public PropertyInfo[] PropertyInfos { get => _propertyInfos; set => _propertyInfos = value ?? Array.Empty<PropertyInfo>(); }
            // �Ԃ�l��null�ɂȂ��foreach���Ŏ��ʂ̂�Array.Empty<T>()��K���Ăяo������
            private PropertyInfo[] _propertyInfos = Array.Empty<PropertyInfo>();

            /// <summary>
            /// �i�[����I�u�W�F�N�g�̌^���擾����
            /// </summary>
            public Type Type { get; private set; }

            /// <summary>
            /// <see cref="Serialization"/>���ŃV���A���C�Y�����I�u�W�F�N�g���擾����
            /// </summary>
            public object Value { get; private set; }

            /// <summary>
            /// �w�肵���I�u�W�F�N�g������<see cref="ReflectionInfo"/>�̐V�����C���X�^���X�𐶐�����
            /// </summary>
            /// <typeparam name="T">�i�[����v�f�̌^</typeparam>
            /// <param name="value">�i�[����v�f</param>
            /// <returns><paramref name="value"/>������<see cref="ReflectionInfo"/>�̐V�����C���X�^���X</returns>
            public static ReflectionInfo Create<T>(T value)
            {
                var result = new ReflectionInfo
                {
                    Type = typeof(T),
                    Value = value
                };
                return result;
            }
        }

        /// <summary>
        /// �ǉ��Ŕ�r����v�f�𐶐�����֐�
        /// </summary>
        /// <param name="obj1">�^������I�u�W�F�N�g1</param>
        /// <param name="obj2">�^������I�u�W�F�N�g2</param>
        /// <returns>�ǉ��Ŕ�r����v�f���i�[����R���N�V����</returns>
        public delegate IEnumerable<OptionalValueEntry> OptionalValueProvider(object obj1, object obj2);

        /// <summary>
        /// �ǉ��Ŕ�r����v�f��\��
        /// </summary>
        public struct OptionalValueEntry
        {
            /// <summary>
            /// �v�f�����擾�܂��͐ݒ肷��
            /// </summary>
            /// <remarks>���O�œf����閼�O�Ȃ̂Ń��O���g��Ȃ��ꍇ�͐ݒ肵�Ȃ���OK</remarks>
            public string Name { get; set; }
            /// <summary>
            /// �i�[����I�u�W�F�N�g�̌^���擾�܂��͐ݒ肷��
            /// </summary>
            public Type Type { get; set; }
            /// <summary>
            /// ��r����l���擾�܂��͐ݒ肷��
            /// </summary>
            public object Value1 { get; set; }
            /// <summary>
            /// ��r����l���擾�܂��͐ݒ肷��
            /// </summary>
            public object Value2 { get; set; }
            public OptionalValueEntry(string name, Type type, object value1, object value2)
            {
                Name = name;
                Type = type;
                Value1 = value1;
                Value2 = value2;
            }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public readonly void Deconstruct(out string name, out Type type, out object value1, out object value2)
            {
                name = Name;
                type = Type;
                value1 = Value1;
                value2 = Value2;
            }
        }
    }
    internal static class ReflectionHelper
    {
        /// <summary>
        /// �����o�[�����o���Ƃ��ɗp����t���O
        /// </summary>
        private const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        /// <summary>
        /// �w�肵���C���^�[�t�F�C�X�������ǂ����𔻒肷��
        /// </summary>
        /// <param name="type">���肷��^</param>
        /// <param name="interfaceType">�������m�F����C���^�[�t�F�C�X�̌^</param>
        /// <returns><paramref name="type"/>��<paramref name="interfaceType"/>���������Ă�����true�C����ȊO��false</returns>
        public static bool HasInterface(this Type type, Type interfaceType) => interfaceType.IsInterface && type.GetInterface(interfaceType.FullName) != null;
        /// <summary>
        /// �w�肵�����O��<see cref="FieldInfo"/>�����o��
        /// </summary>
        /// <param name="info">�t�B�[���h�����o������<see cref="ReflectionSerialize.ReflectionInfo"/>�̃C���X�^���X</param>
        /// <param name="name">���o���t�B�[���h�̖��O</param>
        /// <returns><paramref name="info"/>�ɂ�����<paramref name="name"/>�𖼑O�Ƃ��Ď��t�B�[���h</returns>
        public static FieldInfo GetField(this ReflectionSerialize.ReflectionInfo info, string name) => info.Type.GetField(name, flags) ?? throw new InvalidOperationException($"�t�B�[���h�̓ǂݍ��݂Ɏ��s���܂���\nType: {info.Type.FullName}\nField: {name}");
        /// <summary>
        /// �w�肵�����O��<see cref="MethodInfo"/>�����o��
        /// </summary>
        /// <param name="info">���\�b�h�����o������<see cref="ReflectionSerialize.ReflectionInfo"/>�̃C���X�^���X</param>
        /// <param name="name">���o�����\�b�h�̖��O</param>
        /// <returns><paramref name="info"/>�ɂ�����<paramref name="name"/>�𖼑O�Ƃ��Ď����\�b�h</returns>
        public static MethodInfo GetMethod(this ReflectionSerialize.ReflectionInfo info, string name) => info.Type.GetMethod(name, flags) ?? throw new InvalidOperationException($"���\�b�h�̓ǂݍ��݂Ɏ��s���܂���\nType: {info.Type.FullName}\nMethod: {name}");
        /// <summary>
        /// �w�肵�����O��<see cref="PropertyInfo"/>�����o��
        /// </summary>
        /// <param name="info">�v���p�e�B�����o������<see cref="ReflectionSerialize.ReflectionInfo"/>�̃C���X�^���X</param>
        /// <param name="name">���o���v���p�e�B�̖��O</param>
        /// <returns><paramref name="info"/>�ɂ�����<paramref name="name"/>�𖼑O�Ƃ��Ď��v���p�e�B</returns>
        public static PropertyInfo GetProperty(this ReflectionSerialize.ReflectionInfo info, string name) => info.Type.GetProperty(name, flags) ?? throw new InvalidOperationException($"�v���p�e�B�̓ǂݍ��݂Ɏ��s���܂���\nType: {info.Type.FullName}\nProperty: {name}");
        /// <summary>
        /// ���O�ɓf���e�L�X�g���擾����
        /// </summary>
        /// <param name="value">�e�L�X�g�ɂ���I�u�W�F�N�g</param>
        /// <returns><paramref name="value"/>��\���e�L�X�g</returns>
        public static string ToLogText(this object value)
        {
            if (value == null) return "null";
            switch (value)
            {
                case float v: return $"{v}f";
                case double v: return $"{v}d";
                case decimal v: return $"{v}m";
                case char v: return $"'{v}'";
                case string v: return $"\"{v}\"";
                default: return value.ToString();
            }
        }
        /// <summary>
        /// <see cref="IEnumerable"/>��<see cref="object"/>�^�̔z��ɂ���
        /// </summary>
        /// <param name="enumerable">�z��ɂ���<see cref="IEnumerable"/>�̃C���X�^���X</param>
        /// <exception cref="ArgumentException"><paramref name="enumerable"/>���z��ŁC���̊J�n�C���f�b�N�X��0�ȊO</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/>��null</exception>
        /// <exception cref="RankException"><paramref name="enumerable"/>��2�����ȏ�̔z��</exception>
        /// <returns><paramref name="enumerable"/>���̗v�f���i�[����z��</returns>
        public static object[] ToObjectArray(this IEnumerable enumerable)
        {
            object[] result;
            if (enumerable is ICollection c)
            {
                if (c.Count == 0) return Array.Empty<object>();
                result = new object[c.Count];
                c.CopyTo(result, 0);
                return result;
            }
            var list = new List<object>();
            foreach (var current in list) list.Add(current);
            return list.ToArray();
        }
    }
}
